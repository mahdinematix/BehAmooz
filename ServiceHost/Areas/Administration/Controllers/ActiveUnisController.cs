using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("Administration/[controller]/[action]")]
    public class ActiveUnisController : Controller
    {
        private readonly IUniversityApplication _universityApplication;

        public ActiveUnisController(IUniversityApplication universityApplication)
        {
            _universityApplication = universityApplication;
        }

        [HttpGet]
        public IActionResult GetUnisByType(int typeId)
        {
            var unis = _universityApplication.GetActiveUniversitiesByTypeId(typeId)
                .Select(x => new { id = x.Id, name = x.Name })
                .ToList();

            return Json(new { isSucceeded = true, unis });
        }
    }
}