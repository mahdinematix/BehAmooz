using _01_Framework.Application;
using _02_Query.Contracts.Message;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Professor.Pages
{
    public class IndexModel : UserContextPageModel
    {
        private readonly IMessageQuery _messageQuery;

        public List<MessageQueryModel> ProfessorMessages;
        public IndexModel(IAuthHelper authHelper, IMessageQuery messageQuery):base(authHelper)
        {
            _messageQuery = messageQuery;
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

            ProfessorMessages = _messageQuery.GetProfessorMessages(CurrentAccountUniversityId);

            return Page();
        }
    }
}
