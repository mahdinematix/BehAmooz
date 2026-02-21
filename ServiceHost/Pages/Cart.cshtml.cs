using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CartModel : UserContextPageModel
    {
        public List<CartItem> CartItems;

        public CartModel(IAuthHelper authHelper):base(authHelper)
        {
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