using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        public List<CartItem> CartItems;

        public CartModel(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        private string GetCartCookieName()
        {
            var accountId = _authHelper.CurrentAccountId();
            return $"cart-items-{accountId}";
        }

        public IActionResult OnGet()
        {
            var status = _authHelper.CurrentAccountStatus();

            if (!_authHelper.IsAuthenticated())
                return RedirectToPage("/Login");

            if (_authHelper.CurrentAccountRole() == Roles.Professor)
                return RedirectToPage("/Index", new { area = "Administration" });

            if (status == Statuses.Waiting)
                return RedirectToPage("/NotConfirmed");

            if (status == Statuses.Rejected)
                return RedirectToPage("/Reject");

            var serializer = new JavaScriptSerializer();
            var cookieName = GetCartCookieName();
            var value = Request.Cookies[cookieName];

            if (value != null)
            {
                var cartItems = serializer.Deserialize<List<CartItem>>(value);
                if (cartItems.Count > 0)
                {
                    CartItems = cartItems;
                    return Page();
                }
                return RedirectToPage("/EmptyCart");
            }

            return RedirectToPage("/EmptyCart");
        }

        public IActionResult OnGetRemoveFromCart(long id)
        {
            var serializer = new JavaScriptSerializer();
            var cookieName = GetCartCookieName();
            var value = Request.Cookies[cookieName];
            if (value == null)
                return RedirectToPage("/EmptyCart");

            Response.Cookies.Delete(cookieName);
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
            if (itemToRemove != null)
                cartItems.Remove(itemToRemove);

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(2),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append(cookieName, serializer.Serialize(cartItems), options);
            return RedirectToPage("/Cart");
        }

        public IActionResult OnGetGotoCheckout()
        {
            return RedirectToPage("/Checkout");
        }
    }
}



//using _01_Framework.Application;
//using _01_Framework.Infrastructure;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Nancy.Json;
//using StudyManagement.Application.Contracts.Order;

//namespace ServiceHost.Pages
//{
//    public class CartModel : PageModel
//    {
//        private readonly IAuthHelper _authHelper;
//        public List<CartItem> CartItems;

//        public CartModel(IAuthHelper authHelper)
//        {
//            _authHelper = authHelper;
//        }

//        public const string CookieName = "cart-items";


//        public IActionResult OnGet()
//        {
//            var status = _authHelper.CurrentAccountStatus();

//            if (!_authHelper.IsAuthenticated())
//            {
//                return RedirectToPage("/Login");
//            }

//            if (_authHelper.CurrentAccountRole() == Roles.Professor)
//            {
//                return RedirectToPage("/Index", new { area = "Administration" });
//            }

//            if (status == Statuses.Waiting)
//            {
//                return RedirectToPage("/NotConfirmed");
//            }

//            if (status == Statuses.Rejected)
//            {
//                return RedirectToPage("/Reject");
//            }

//            var serializer = new JavaScriptSerializer();
//            var value = Request.Cookies[CookieName];
//            if (value != null)
//            {
//                var cartItems = serializer.Deserialize<List<CartItem>>(value);
//                if (cartItems.Count > 0)
//                {
//                    foreach (var cartItem in cartItems)
//                    {
//                        var cartItemUnitPrice = cartItem.SessionPrice;
//                    }
//                    CartItems = cartItems;

//                    return Page();
//                }
//                return RedirectToPage("/EmptyCart");
//            }
//            return RedirectToPage("/EmptyCart");

//        }

//        public IActionResult OnGetRemoveFromCart(long id)
//        {
//            var serializer = new JavaScriptSerializer();
//            var value = Request.Cookies[CookieName];
//            Response.Cookies.Delete(CookieName);
//            var cartItems = serializer.Deserialize<List<CartItem>>(value);
//            var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
//            cartItems.Remove(itemToRemove);
//            var options = new CookieOptions { Expires = DateTime.Now.AddDays(2), IsEssential = true, SameSite = SameSiteMode.Lax};
//            Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), options);
//            return RedirectToPage("/Cart");
//        }


//        public IActionResult OnGetGotoCheckout()
//        {

//            return RedirectToPage("/Checkout");
//        }
//    }
//}
