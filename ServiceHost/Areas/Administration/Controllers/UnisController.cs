using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class UnisController : Controller
    {
        private readonly IUniversityApplication _universityApplication;

        public UnisController(IUniversityApplication universityApplication)
        {
            _universityApplication = universityApplication;
        }

        [HttpGet]
        public JsonResult GetUnisByType(string typeId)
        {
            List<SelectListItem> unis = GetUnis(Convert.ToInt32(typeId));
            return Json(unis);
        }

        private List<SelectListItem> GetUnis(int typeId = 0)
        {
            List<SelectListItem> lstUnis = _universityApplication.GetUniversitiesByType(typeId)
                .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();

            var defItem = new SelectListItem
            {
                Value = "0",
                Text = ApplicationMessages.SelectYourUniversity
            };

            lstUnis.Insert(0, defItem);

            return lstUnis;
        }
    }
}
