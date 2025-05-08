using _01_Framework.Application;

namespace MessageManagement.Application.Contract.Message
{
    public interface IMessageApplication
    {
        OperationResult Create(CreateMessage command);
        OperationResult Edit(EditMessage command);
        EditMessage GetDetails(long id);
        List<MessageViewModel> Search(MessageSearchModel searchModel);
    }
}
