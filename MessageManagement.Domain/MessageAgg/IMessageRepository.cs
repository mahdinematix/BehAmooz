using _01_Framework.Domain;
using MessageManagement.Application.Contract.Message;

namespace MessageManagement.Domain.MessageAgg
{
    public interface IMessageRepository : IRepositoryBase<long, Message>
    {
        EditMessage GetDetails(long id);
        List<MessageViewModel> Search(MessageSearchModel searchModel);
    }
}
