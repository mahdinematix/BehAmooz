using System.Globalization;
using _01_Framework.Application;
using _01_Framework.Infrastructure;
using MessageManagement.Application.Contract.Message;
using MessageManagement.Domain.MessageAgg;

namespace MessageManagement.Infrastructure.EFCore.Repository
{
    public class MessageRepository : RepositoryBase<long, Message> , IMessageRepository
    {
        private readonly MessageContext _context;

        public MessageRepository(MessageContext context) : base(context)
        {
            _context = context;
        }

        public EditMessage GetDetails(long id)
        {
            return _context.Messages.Select(x => new EditMessage
            {
                Id = x.Id,
                Body = x.Body,
                Title = x.Title,
                StartDate = x.StartDate.ToString(CultureInfo.InvariantCulture),
                EndDate = x.EndDate.ToString(CultureInfo.InvariantCulture),
                MessageFor = x.MessageFor
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<MessageViewModel> Search(MessageSearchModel searchModel)
        {
            var query = _context.Messages.Select(x => new MessageViewModel
            {
                Id = x.Id,
                Body = x.Body,
                Title = x.Title,
                StartDate = x.StartDate.ToFarsi(),
                EndDate = x.EndDate.ToFarsi(),
                StartDateGr = x.StartDate,
                EndDateGr = x.EndDate,
                MessageFor = x.MessageFor,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Title))
            {
                query = query.Where(x => x.Title.Contains(searchModel.Title));
            }

            if (searchModel.MessageFor != "0" && searchModel.MessageFor != null)
            {
                query = query.Where(x => x.MessageFor == searchModel.MessageFor);
            }


            return query.OrderByDescending(x => x.Id).ToList();


        }
    }
}
