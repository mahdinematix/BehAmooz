using _02_Query.Contracts.Message;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.ViewComponents
{
    public class ProfessorsMessagesViewComponent : ViewComponent
    {
        private readonly IMessageQuery _messageQuery;

        public ProfessorsMessagesViewComponent(IMessageQuery messageQuery)
        {
            _messageQuery = messageQuery;
        }

        public IViewComponentResult Invoke()
        {
            var professorMessages = _messageQuery.GetProfessorMessages();
            return View(professorMessages);
        }
    }
}
