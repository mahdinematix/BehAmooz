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

        public List<MessageQueryModel> GetProfessorMessages()
        {
            return _messageContext.Messages.Where(x => x.MessageFor == "اساتید").Where(x=>x.EndDate >= DateTime.Now).Select(x => new MessageQueryModel
                {
                    Title = x.Title,
                    Body = x.Body,
                    CreationDate = x.CreationDate.ToFarsi()
                })
                .ToList();
        }
    }
}
