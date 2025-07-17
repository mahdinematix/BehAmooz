using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems;
        public const string CookieName = "cart-items";


        public IActionResult OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            if (value != null)
            {
                var cartItems = serializer.Deserialize<List<CartItem>>(value);
                if (cartItems.Count > 0)
                {
                    foreach (var cartItem in cartItems)
                    {
                        var cartItemUnitPrice = cartItem.SessionPrice;
                    }
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
            var value = Request.Cookies[CookieName];
            Response.Cookies.Delete(CookieName);
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
            cartItems.Remove(itemToRemove);
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(2) };
            Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), options);
            return RedirectToPage("/Cart");
        }

        public IActionResult OnGetGotoCheckout()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            foreach (var cartItem in cartItems)
            {
                var cartItemUnitPrice = cartItem.SessionPrice;
            }

            return RedirectToPage("/Checkout");
        }
    }
}
