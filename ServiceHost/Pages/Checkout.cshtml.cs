using _01_Framework.Application;
using _01_Framework.Application.ZarinPal;
using _01_Framework.Infrastructure;
using _02_Query.Contracts;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CheckoutModel : UserContextPageModel
    {
        private readonly ICartCalculatorService _cartCalculatorService;
        public Cart Cart;
        private readonly ICartService _cartService;
        private readonly IOrderApplication _orderApplication;
        private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IWalletApplication _walletApplication;
        [TempData] public string Message { get; set; }

        public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService, IOrderApplication orderApplication, IAuthHelper authHelper,
            IZarinPalFactory zarinPalFactory, IWalletApplication walletApplication) : base(authHelper)
        {
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _orderApplication = orderApplication;
            _zarinPalFactory = zarinPalFactory;
            _walletApplication = walletApplication;
            Cart = new Cart();
        }

        private string GetCartCookieName()
        {
            return $"cart-items-{CurrentAccountId}";
        }

        public IActionResult OnGet()
        {
            if (!IsAuthenticated)
                return RedirectToPage("/Login");

            if (CurrentAccountRole == Roles.Professor)
                return RedirectToPage("/Index", new { area = "Administration" });

            if (CurrentAccountStatus == Statuses.Waiting)
                return RedirectToPage("/NotConfirmed");

            if (CurrentAccountStatus == Statuses.Rejected)
                return RedirectToPage("/Reject");
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[GetCartCookieName()];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);

            if (cartItems == null)
            {
                return RedirectToPage("/EmptyCart");
            }

            Cart = _cartCalculatorService.ComputeCart(cartItems);
            _cartService.Set(Cart);

            return Page();
        }

        public IActionResult OnPostPay()
        {
            var cart = _cartService.Get();


            var orderId = _orderApplication.PlaceOrder(cart, CurrentAccountId);
            var orderAmount = _orderApplication.GetAmountBy(orderId).ToString();


            var paymentData = _zarinPalFactory.CreatePaymentRequest(
                orderAmount, CurrentAccountInfo.Mobile, CurrentAccountInfo.Email,
                $"Buy Session with orderId:{orderId}", orderId, PaymentTypes.BuyFromGateway);

            return Redirect(
                $"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentData.authority}");
        }

        public IActionResult OnPostPayWithWallet()
        {
            var cart = _cartService.Get();

            var orderId = _orderApplication.PlaceOrder(cart, CurrentAccountId);
            var command = new BuyFromWalletDto
            {
                AccountId = CurrentAccountId,
                Amount = Convert.ToInt64(cart.FinalAmount)
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
            var value = Request.Cookies[GetCartCookieName()];
            Response.Cookies.Delete(GetCartCookieName());
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            cartItems.RemoveAll(x => x.SessionPrice > 0);
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(2), IsEssential = true, SameSite = SameSiteMode.Lax };
            Response.Cookies.Append(GetCartCookieName(), serializer.Serialize(cartItems), options);
            return RedirectToPage("/PaymentResult",
                paymentResult.Succeeded(ApplicationMessages.PaymentByWallet, "0"));
        }

        public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status,
            [FromQuery] long oId)
        {
            var orderAmount = _orderApplication.GetAmountBy(oId);
            var verificationResponse =
                _zarinPalFactory.CreateVerificationRequest(authority,
                    orderAmount.ToString());
            var result = new PaymentResult();
            if (status == "OK" && verificationResponse.code >= 100)
            {
                var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verificationResponse.ref_id);
                var serializer = new JavaScriptSerializer();
                var value = Request.Cookies[GetCartCookieName()];
                Response.Cookies.Delete(GetCartCookieName());
                var cartItems = serializer.Deserialize<List<CartItem>>(value);
                cartItems.RemoveAll(x => x.SessionPrice > 0);
                var options = new CookieOptions { Expires = DateTime.Now.AddDays(2), IsEssential = true, SameSite = SameSiteMode.Lax };
                Response.Cookies.Append(GetCartCookieName(), serializer.Serialize(cartItems), options);
                result = result.Succeeded(ApplicationMessages.PaymentSuccessful, issueTrackingNo);
                var command = new BuyFromGatewayDto
                {
                    AccountId = CurrentAccountId,
                    Amount = orderAmount,
                    OrderId = oId,
                    Description = $"{issueTrackingNo}"
                };

                _walletApplication.BuyFromGateway(command);
                return RedirectToPage("/PaymentResult", result);
            }

            result = result.Failed(ApplicationMessages.PaymentFailed);
            return RedirectToPage("/PaymentResult", result);

        }
    }
}
