using _02_Query.Contracts.Message;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages.ViewComponents
{
    public class StudentMessagesViewComponent :ViewComponent
    {
        private readonly IMessageQuery _messageQuery;

        public StudentMessagesViewComponent(IMessageQuery messageQuery)
        {
            _messageQuery = messageQuery;
        }

        public IViewComponentResult Invoke()
        {
            var studentMessages = _messageQuery.GetStudentMessages();
            return View(studentMessages);
        }
    }
}
