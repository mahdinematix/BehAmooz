using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Customer;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Financial
{
    public class ListCustomersModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly ICustomerQuery _customerQuery;

        public List<CustomerQueryModel> Customers;
        public CustomerSearchModel SearchModel;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;

        public ListCustomersModel(ICustomerQuery customerQuery, IAuthHelper authHelper)
        {
            _customerQuery = customerQuery;
            _authHelper = authHelper;
        }

        public void OnGet(CustomerSearchModel searchModel)
        {
            Customers = _customerQuery.GetCustomersByProfessorId(searchModel, _authHelper.CurrentAccountId());
            UniTypes = GetUniTypes();
            Unis = GetUnis();
        }


        private List<SelectListItem> GetUnis(int typeId = 1)
        {

            List<SelectListItem> lstUnis = Universities.List
                .Where(c => c.UniversityTypeId == typeId)
                .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = "œ«‰‘ê«Â —« «‰ Œ«» ò‰?œ"
            };

            lstUnis.Insert(0, defItem);

            return lstUnis;
        }

        private List<SelectListItem> GetUniTypes()
        {
            var lstCountries = new List<SelectListItem>();

            List<UniversityTypeViewModel> Countries = UniversityTypes.List;

            lstCountries = Countries.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = "‰Ê⁄ œ«‰‘ê«Â —« «‰ Œ«» ò‰?œ"
            };

            lstCountries.Insert(0, defItem);

            return lstCountries;
        }

    }
}
