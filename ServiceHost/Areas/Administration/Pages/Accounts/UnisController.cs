using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts
{
    public class UnisController : Controller
    {
        [HttpGet]
        public JsonResult GetUnisByType(string typeId)
        {
            List<SelectListItem> unis = GetUnis(Convert.ToInt32(typeId));
            return Json(unis);
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

            var defItem = new SelectListItem
            {
                Value = "0",
                Text = "دانشگاه را انتخاب کنید"
            };

            lstUnis.Insert(0, defItem);

            return lstUnis;
        }
    }
}
