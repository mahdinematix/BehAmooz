using _01_Framework.Application;
using _01_Framework.Infrastructure;
using MessageManagement.Application.Contract.Message;
using MessageManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Administration.Pages.Message
{
    public class IndexModel : UserContextPageModel
    {
        private readonly IMessageApplication _messageApplication;
        private readonly IUniversityApplication _universityApplication;

        public MessageSearchModel SearchModel;
        public List<MessageViewModel> Messages;
        public SelectList Universities;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public bool ShowUniversityField { get; set; }

        public IndexModel(IMessageApplication messageApplication, IAuthHelper authHelper, IUniversityApplication universityApplication) :base(authHelper)
        {
            _messageApplication = messageApplication;
            _universityApplication = universityApplication;
        }

        [NeedsPermissions(MessagePermissions.ListMessages)]
        public IActionResult OnGet(MessageSearchModel searchModel)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            if (CurrentAccountRole == Roles.SuperAdministrator)
            {
                Universities = new SelectList(_universityApplication.GetActiveUniversities(), "Id", "Name");
            }

            ShowUniversityField = CurrentAccountRole == Roles.SuperAdministrator;
            Messages = _messageApplication.Search(searchModel,CurrentAccountRole, CurrentAccountUniversityId);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
            return Page();
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateMessage();
            if (CurrentAccountRole==Roles.SuperAdministrator)
            {
                command = new CreateMessage
                {
                    UniversityTypes = UniversityTypes.List,
                    Universities = new List<UniversityViewModel>()
                };
            }
            else
            {
                command = new CreateMessage
                {
                    UniversityTypeId = CurrentAccountInfo.UniversityTypeId,
                    UniversityId = CurrentAccountUniversityId
                };
            }
            
            return Partial("./Create", command);
        }

        [NeedsPermissions(MessagePermissions.CreateMessage)]
        public IActionResult OnPostCreate(CreateMessage command)
        {
            if (CurrentAccountRole == Roles.SuperAdministrator && command.ForAllUniversities)
            {
                command.UniversityTypeId = 0;
                command.UniversityId = 0;
            }
            var result = _messageApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var message = _messageApplication.GetDetails(id);
            if (CurrentAccountRole == Roles.SuperAdministrator)
            {
                message.UniversityTypes = UniversityTypes.List;
                message.Universities = _universityApplication
                    .GetActiveUniversitiesByTypeId(message.UniversityTypeId);
            }
            return Partial("Edit", message);
        }

        [NeedsPermissions(MessagePermissions.EditMessage)]
        public IActionResult OnPostEdit(EditMessage command)
        {
            var result = _messageApplication.Edit(command);
            return new JsonResult(result);
        }

        private List<SelectListItem> GetUnis(int typeId = 0)
        {

            List<SelectListItem> lstUnis = _universityApplication.GetActiveUniversitiesByTypeId(typeId).Select(n =>
                new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = ApplicationMessages.SelectYourUniversity
            };

            lstUnis.Insert(0, defItem);

            return lstUnis;
        }

        private List<SelectListItem> GetUniTypes()
        {
            var lstCountries = new List<SelectListItem>();

            List<UniversityTypeViewModel> Countries = UniversityTypes.List;

            lstCountries = Countries.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = ApplicationMessages.SelectYourUniversityType
            };

            lstCountries.Insert(0, defItem);

            return lstCountries;
        }
    }
}
