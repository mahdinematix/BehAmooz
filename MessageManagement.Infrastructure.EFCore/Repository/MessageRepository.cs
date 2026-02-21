using _01_Framework.Application;
using _01_Framework.Infrastructure;
using MessageManagement.Application.Contract.Message;
using MessageManagement.Domain.MessageAgg;
using StudyManagement.Infrastructure.EFCore;
using System.Globalization;

namespace MessageManagement.Infrastructure.EFCore.Repository
{
    public class MessageRepository : RepositoryBase<long, Message> , IMessageRepository
    {
        private readonly MessageContext _context;
        private readonly StudyContext _studyContext; 

        public MessageRepository(MessageContext context, StudyContext studyContext) : base(context)
        {
            _context = context;
            _studyContext = studyContext;
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
                MessageFor = x.MessageFor,
                UniversityTypeId = x.UniversityTypeId,
                ForAllUniversities = x.ForAllUniversities,
                UniversityId = x.UniversityId
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<MessageViewModel> Search(MessageSearchModel searchModel, string currentAccountRole, long currentAccountUniversityId)
        {
            var uniDict = _studyContext.Universities
                .Where(u => u.IsActive)
                .Select(u => new { u.Id, u.Name })
                .ToDictionary(x => x.Id, x => x.Name);


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
                ForAllUniversities = x.ForAllUniversities,
                UniversityTypeId = x.UniversityTypeId,
                UniversityId = x.UniversityId,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (currentAccountRole==Roles.Administrator)
            {
                query = query.Where(x => x.UniversityId == currentAccountUniversityId);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Title))
            {
                query = query.Where(x => x.Title.Contains(searchModel.Title));
            }

            if (searchModel.MessageFor != "0" && searchModel.MessageFor != null)
            {
                query = query.Where(x => x.MessageFor == searchModel.MessageFor);
            }

            if (searchModel.ForAllUniversities)
                query = query.Where(x => x.ForAllUniversities);

            if (searchModel.UniversityTypeId > 0)
                query = query.Where(x => x.UniversityTypeId == searchModel.UniversityTypeId);

            if (searchModel.UniversityId >0)
            {
                query = query.Where(x => x.UniversityId == searchModel.UniversityId);
            }

            var list = query.ToList();

            foreach (var item in list)
            {
                item.UniversityName = uniDict.TryGetValue(item.UniversityId, out var name) ? name : "";
            }


            return list.OrderByDescending(x => x.Id).ToList();


        }
    }
}
