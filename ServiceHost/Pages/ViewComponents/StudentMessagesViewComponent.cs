using _01_Framework.Application;
using _02_Query.Contracts.Message;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages.ViewComponents
{
    public class StudentMessagesViewComponent :ViewComponent
    {
        private readonly IMessageQuery _messageQuery;
        private readonly IAuthHelper _authHelper;

        public StudentMessagesViewComponent(IMessageQuery messageQuery, IAuthHelper authHelper)
        {
            _messageQuery = messageQuery;
            _authHelper = authHelper;
        }

        public IViewComponentResult Invoke()
        {
            var studentMessages = _messageQuery.GetStudentMessages(_authHelper.CurrentAccountUniversityId());
            return View(studentMessages);
        }
    }
}
