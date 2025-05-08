using _01_Framework.Application;
using MessageManagement.Application.Contract.Message;
using MessageManagement.Domain.MessageAgg;

namespace MessageManagement.Application
{
    public class MessageApplication : IMessageApplication
    {
        private readonly IMessageRepository _messageRepository;

        public MessageApplication(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public OperationResult Create(CreateMessage command)
        {
            var operation = new OperationResult();
            if (_messageRepository.Exists(x=>x.Title == command.Title))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            var message = new Message(command.Title, command.Body, command.MessageFor, startDate, endDate);
            _messageRepository.Create(message);
            _messageRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Edit(EditMessage command)
        {
            var operation = new OperationResult();
            var message = _messageRepository.GetBy(command.Id);
            if (message == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (_messageRepository.Exists(x=>x.Title ==command.Title && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }
            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            message.Edit(command.Title, command.Body, command.MessageFor, startDate, endDate);
            _messageRepository.Save();
            return operation.Succeed();
        }

        public EditMessage GetDetails(long id)
        {
            return _messageRepository.GetDetails(id);
        }

        public List<MessageViewModel> Search(MessageSearchModel searchModel)
        {
            return _messageRepository.Search(searchModel);
        }
    }
}
