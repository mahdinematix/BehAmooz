using _01_Framework.Application;
using _02_Query.Contracts.Message;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.ViewComponents
{
    public class MessagesViewComponent : ViewComponent
    {
        private readonly IMessageQuery _messageQuery;
        private readonly IAuthHelper _authHelper;

        public MessagesViewComponent(IMessageQuery messageQuery, IAuthHelper authHelper)
        {
            _messageQuery = messageQuery;
            _authHelper = authHelper;
        }

        public IViewComponentResult Invoke()
        {
            var messages = _messageQuery.GetAdminMessages(_authHelper.CurrentAccountUniversityId());
            return View(messages);
        }
    }
}
