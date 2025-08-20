using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class MySessionsModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly IOrderItemQuery _orderItemQuery;
        public List<OrderItemQueryModel> OrderItems;

        public MySessionsModel(IAuthHelper authHelper, IOrderItemQuery orderItemQuery)
        {
            _authHelper = authHelper;
            _orderItemQuery = orderItemQuery;
        }


        public IActionResult OnGet()
        {

            var status = _authHelper.CurrentAccountStatus();

            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (_authHelper.CurrentAccountRole() != Roles.Student)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Rejected");
            }

            OrderItems = _orderItemQuery.GetItemsThatPaid();

            return Page();
        }
    }
}
