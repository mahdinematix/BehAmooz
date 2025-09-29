using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Message;
using MessageManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class MessageQuery : IMessageQuery
    {
        private readonly MessageContext _messageContext;
        private readonly IAuthHelper _authHelper;

        public MessageQuery(MessageContext messageContext, IAuthHelper authHelper)
        {
            _messageContext = messageContext;
            _authHelper = authHelper;
        }

        public List<MessageQueryModel> GetAdminMessages()

        {
            if (_authHelper.CurrentAccountRole() == Roles.Professor)
            {
                return _messageContext.Messages.Where(x => x.MessageFor == "اساتید").Where(x => x.EndDate >= DateTime.Now).Select(x => new MessageQueryModel
                {
                    Title = x.Title,
                    Body = x.Body,
                    CreationDate = x.CreationDate.ToFarsi()
                })
                    .ToList();
            }

            return _messageContext.Messages.Where(x => x.MessageFor == "مدیر سیستم ها").Where(x => x.EndDate >= DateTime.Now).Select(x => new MessageQueryModel
            {
                Title = x.Title,
                Body = x.Body,
                CreationDate = x.CreationDate.ToFarsi()
            })
                .ToList();
        }

        public List<MessageQueryModel> GetStudentMessages()
        {
            return _messageContext.Messages.Where(x => x.MessageFor == "دانشجویان").Where(x => x.EndDate >= DateTime.Now).Select(x => new MessageQueryModel
            {
                Title = x.Title,
                Body = x.Body,
                CreationDate = x.CreationDate.ToFarsi()
            })
                .ToList();
        }
    }
}
