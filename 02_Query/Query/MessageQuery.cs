using _01_Framework.Application;
using _02_Query.Contracts.Message;
using MessageManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class MessageQuery : IMessageQuery
    {
        private readonly MessageContext _messageContext;

        public MessageQuery(MessageContext messageContext)
        {
            _messageContext = messageContext;
        }

        public List<MessageQueryModel> GetAdminMessages(long currentAccountUniversity)

        {
            return _messageContext.Messages.Where(x => x.MessageFor == "مدیر سیستم ها").Where(x => x.EndDate >= DateTime.Now).Where(x=>x.UniversityId==currentAccountUniversity || x.ForAllUniversities).Select(x => new MessageQueryModel
            {
                Title = x.Title,
                Body = x.Body,
                CreationDate = x.CreationDate.ToFarsi()
            })
                .ToList();
        }

        public List<MessageQueryModel> GetProfessorMessages(long currentAccountUniversity)
        {

            return _messageContext.Messages.Where(x => x.MessageFor == "اساتید").Where(x => x.EndDate >= DateTime.Now).Where(x => x.UniversityId == currentAccountUniversity || x.ForAllUniversities).Select(x => new MessageQueryModel
            {
                Title = x.Title,
                Body = x.Body,
                CreationDate = x.CreationDate.ToFarsi()
            })
                .ToList();
        }


        public List<MessageQueryModel> GetStudentMessages(long currentAccountUniversity)
        {
            return _messageContext.Messages.Where(x => x.MessageFor == "دانشجویان").Where(x => x.EndDate >= DateTime.Now).Where(x => x.UniversityId == currentAccountUniversity || x.ForAllUniversities).Select(x => new MessageQueryModel
            {
                Title = x.Title,
                Body = x.Body,
                CreationDate = x.CreationDate.ToFarsi()
            })
                .ToList();
        }
    }
}
