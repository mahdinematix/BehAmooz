using MessageManagement.Application.Contract.Message;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Message
{
    public class IndexModel : PageModel
    {
        private readonly IMessageApplication _messageApplication;

        public MessageSearchModel SearchModel;
        public List<MessageViewModel> Messages;

        public IndexModel(IMessageApplication messageApplication)
        {
            _messageApplication = messageApplication;
        }

        public void OnGet(MessageSearchModel searchModel)
        {
            Messages = _messageApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateMessage());
        }

        public IActionResult OnPostCreate(CreateMessage command)
        {
            var result = _messageApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var message = _messageApplication.GetDetails(id);
            return Partial("Edit", message);
        }

        public IActionResult OnPostEdit(EditMessage command)
        {
            var result = _messageApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
