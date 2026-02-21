using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Professor.Controllers
{
    [Area("Professor")]
    [Route("Professor/[controller]/[action]")]
    public class UnisController : Controller
    {
        private readonly IUniversityApplication _universityApplication;

        public UnisController(IUniversityApplication universityApplication)
        {
            _universityApplication = universityApplication;
        }

        [HttpGet]
        public IActionResult GetUnisByType(int typeId)
        {
            if (typeId <= 0)
            {
                return Ok(new[]
                {
                    new { value = "0", text = ApplicationMessages.SelectYourUniversityType }
                });
            }

            var unis = _universityApplication.GetActiveUniversitiesByTypeId(typeId)
                .Select(x => new { value = x.Id.ToString(), text = x.Name })
                .ToList();

            unis.Insert(0, new { value = "0", text = ApplicationMessages.SelectYourUniversity });

            return Ok(unis);
        }
    }
}