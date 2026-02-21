using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Order;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class MySessionsModel : UserContextPageModel
    {
        private readonly IOrderQuery _orderQuery;
        public List<OrderItemQueryModel> OrderItems;

        public MySessionsModel(IAuthHelper authHelper, IOrderQuery orderQuery):base(authHelper)
        {
            _orderQuery = orderQuery;
        }


        public IActionResult OnGet()
        {
            if (!IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (CurrentAccountRole == Roles.Professor)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }

            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            OrderItems = _orderQuery.GetItemsThatPaid();

            return Page();
        }
    }
}
