using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Customer;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Professor.Pages.Financial
{
    public class ListCustomersModel : UserContextPageModel
    {
        private readonly ICustomerQuery _customerQuery;
        private readonly IUniversityApplication _universityApplication;

        public List<CustomerQueryModel> Customers;

        [BindProperty(SupportsGet = true)]
        public CustomerSearchModel SearchModel { get; set; }

        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;

        public ListCustomersModel(ICustomerQuery customerQuery, IAuthHelper authHelper, IUniversityApplication universityApplication):base(authHelper)
        {
            _customerQuery = customerQuery;
            _universityApplication = universityApplication;
        }

        public IActionResult OnGet()
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Customers = _customerQuery.GetCustomersByProfessorId(SearchModel, CurrentAccountId);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
            return Page();
        }

        public IActionResult OnGetExportToExcel(CustomerSearchModel searchModel)
        {

            Customers = _customerQuery.GetCustomersByProfessorId(searchModel, CurrentAccountId);

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Customers");

                int col = 1;

                ws.Cell(1, col++).Value = "نام";
                ws.Cell(1, col++).Value = "کد دانشجو";
                ws.Cell(1, col++).Value = "دانشگاه";
                ws.Cell(1, col++).Value = "درس";
                ws.Cell(1, col++).Value = "کد کلاس";
                ws.Cell(1, col++).Value = "روز کلاس";
                ws.Cell(1, col++).Value = "ساعت کلاس";

                for (int i = 1; i <= 16; i++)
                {
                    ws.Cell(1, col++).Value = $"جلسه {i}";
                }

                ws.Cell(1, col++).Value = "جمع جلسات";
                ws.Cell(1, col++).Value = "مبلغ هر جلسه (تومان)";
                ws.Cell(1, col++).Value = "جمع سفارشات (تومان)";
                ws.Cell(1, col++).Value = "سهم سازمان (تومان)";
                ws.Cell(1, col++).Value = "مالیات (تومان)";
                ws.Cell(1, col++).Value = "سهم استاد (تومان)";

                var headerRange = ws.Range(1, 1, 1, col - 1);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 2;
                foreach (var c in Customers)
                {
                    col = 1;
                    ws.Cell(row, col++).Value = c.FullName;
                    ws.Cell(row, col++).Value = c.Code;
                    ws.Cell(row, col++).Value = _universityApplication.GetNameBy(c.UniversityId);
                    ws.Cell(row, col++).Value = c.CourseName;
                    ws.Cell(row, col++).Value = c.ClassCode;
                    ws.Cell(row, col++).Value = Days.GetName(c.ClassDay);
                    ws.Cell(row, col++).Value = $"{c.ClassStartTime} تا {c.ClassEndTime}";

                    
                    for (int i = 1; i <= 16; i++)
                    {
                        bool attended = false;

                        if (c.SessionCounts != null && c.SessionCounts.TryGetValue(i, out int value))
                        {
                            attended = value > 0;
                        }

                        ws.Cell(row, col++).Value = attended ? "*" : "";
                    }

                    ws.Cell(row, col++).Value = c.TotalSessions;
                    ws.Cell(row, col++).Value = c.SessionPrice;
                    ws.Cell(row, col++).Value = c.TotalAmount;
                    ws.Cell(row, col++).Value = c.OrganShare;
                    ws.Cell(row, col++).Value = c.Tax;
                    ws.Cell(row, col++).Value = c.ProfessorShare;

                    row++;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Customers.xlsx");
                }
            }
        }

        private List<SelectListItem> GetUnis(int typeId = 0)
        {

            List<SelectListItem> lstUnis = _universityApplication.GetUniversitiesByType(typeId).Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = ApplicationMessages.SelectYourUniversity
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
                Text = ApplicationMessages.SelectYourUniversityType
            };

            lstCountries.Insert(0, defItem);

            return lstCountries;
        }

    }
}
