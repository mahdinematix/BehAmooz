using _01_Framework.Application;
using _01_Framework.Application.ZarinPal;
using _02_Query.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using StudyManagement.Application.Contracts.Order;
using System.Globalization;
using AccountManagement.Application.Contract.Wallet;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly ICartCalculatorService _cartCalculatorService;
        public const string CookieName = "cart-items";
        public Cart Cart;
        private readonly ICartService _cartService;
        private readonly IOrderApplication _orderApplication;
        private readonly IAuthHelper _authHelper;
        private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IWalletApplication _walletApplication;
        [TempData] public string Message { get; set; }

        public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService, IOrderApplication orderApplication, IAuthHelper authHelper,
            IZarinPalFactory zarinPalFactory, IWalletApplication walletApplication)
        {
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _orderApplication = orderApplication;
            _authHelper = authHelper;
            _zarinPalFactory = zarinPalFactory;
            _walletApplication = walletApplication;
            Cart = new Cart();
        }

        public void OnGet()
        {
            //var status = _authHelper.CurrentAccountStatus();

            //if (!_authHelper.IsAuthenticated())
            //{
            //    return RedirectToPage("/Login");
            //}

            //if (_authHelper.CurrentAccountRole() != Roles.Student)
            //{
            //    return RedirectToPage("/Index", new { area = "Administration" });
            //}

            //if (status == Statuses.Waiting)
            //{
            //    return RedirectToPage("/NotConfirmed");
            //}

            //if (status == Statuses.Reject)
            //{
            //    return RedirectToPage("/Reject");
            //}
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            

            //if (status == Statuses.Confirmed)
            //{
            //    if (cartItems == null)
            //    {
            //        return RedirectToPage("EmptyCart");
            //    }
            //}

            Cart = _cartCalculatorService.ComputeCart(cartItems);
            _cartService.Set(Cart);

            //return Page();
        }

        public IActionResult OnPostPay()
        {
            var cart = _cartService.Get();


            var orderId = _orderApplication.PlaceOrder(cart);
            //if (paymentMethod == PaymentMethods.Zarinpal)
            //{
            //    var paymentResponse = _zarinPalFactory.CreatePaymentRequest(
            //        cart.TotalAmount.ToString(CultureInfo.InvariantCulture), "", "",
            //        "خرید از درگاه لوازم خانگی و دکوری", orderId);

            //    return Redirect(
            //        $"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");
            //}

            
            _orderApplication.PaymentSucceeded(orderId, 0);

            var command = new BuyFromWalletDto
            {
                AccountId = _authHelper.CurrentAccountId(),
                Amount = Convert.ToInt64(cart.TotalAmount)
            };
            _walletApplication.BuyFromGateway(command);
            
            var paymentResult = new PaymentResult();
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            Response.Cookies.Delete(CookieName);
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            cartItems.RemoveAll(x => x.SessionPrice > 0);
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(2), IsEssential = true, SameSite = SameSiteMode.Lax };
            Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), options);
            return RedirectToPage("/PaymentResult",
                paymentResult.Succeeded(ApplicationMessages.PaymentByCash, "0"));
        }

        public IActionResult OnPostPayWithWallet()
        {
            var cart = _cartService.Get();

            var orderId = _orderApplication.PlaceOrder(cart);
            var command = new BuyFromWalletDto
            {
                AccountId = _authHelper.CurrentAccountId(),
                Amount = Convert.ToInt64(cart.TotalAmount)
            };
            var result = _walletApplication.BuyFromWallet(command);
            if (!result.IsSucceeded)
            {
                Message = result.Message;
                return RedirectToPage();
            }
            _orderApplication.PaymentSucceeded(orderId, 0);

            var paymentResult = new PaymentResult();
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            Response.Cookies.Delete(CookieName);
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            cartItems.RemoveAll(x => x.SessionPrice > 0);
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(2), IsEssential = true, SameSite = SameSiteMode.Lax };
            Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), options);
            return RedirectToPage("/PaymentResult",
                paymentResult.Succeeded(ApplicationMessages.PaymentByCash, "0"));
        }

        public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status,
            [FromQuery] long oId)
        {
            var orderAmount = _orderApplication.GetAmountBy(oId);
            var verificationResponse =
                _zarinPalFactory.CreateVerificationRequest(authority,
                    orderAmount.ToString(CultureInfo.InvariantCulture));
            var result = new PaymentResult();
            if (status == "OK" && verificationResponse.Status >= 100)
            {
                var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verificationResponse.RefID);
                Response.Cookies.Delete(CookieName);
                result = result.Succeeded(ApplicationMessages.PaymentSuccessful, issueTrackingNo);
                return RedirectToPage("/PaymentResult", result);
            }

            result = result.Failed(ApplicationMessages.PaymentFailed);
            return RedirectToPage("/PaymentResult", result);

        }
    }
}
