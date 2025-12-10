using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IAuthHelper _authHelper;
        private readonly IOrderApplication _orderApplication;
        public List<OrderQueryModel> Orders;

        public OrdersModel(IAuthHelper authHelper, IOrderQuery orderQuery, IOrderApplication orderApplication)
        {
            _authHelper = authHelper;
            _orderQuery = orderQuery;
            _orderApplication = orderApplication;
        }

        public IActionResult OnGet()
        {
            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }
            if (_authHelper.CurrentAccountRole() != Roles.Student)
            {
                return RedirectToPage("/Financial/Log", new { area = "Administration" });
            }
            var status = _authHelper.CurrentAccountStatus();

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            Orders = _orderQuery.GetOrdersThatPaid();


            return Page();
        }
        public IActionResult OnGetItems(long id)
        {
            var items = _orderApplication.GetItems(id);
            return Partial("Items", items);
        }
    }
}
