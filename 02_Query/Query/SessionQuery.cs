using _02_Query.Contracts.Session;
using _02_Query.Contracts.SessionPicture;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.Order;
using StudyManagement.Domain.SessionPictureAgg;
using StudyManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class SessionQuery : ISessionQuery
    {

        private readonly StudyContext _studyContext;

        public SessionQuery(StudyContext studyContext)
        {
            _studyContext = studyContext;
        }

        public List<CartItem> GetItemsByClassId(long classId)
        {
            return _studyContext.Sessions.Include(x => x.Class).ThenInclude(x => x.Course).Where(x=>x.ClassId == classId).Select(x => new CartItem
            {
                SessionPrice = x.Price,
                SessionNumber = x.Number,
                ClassStartTime = x.Class.StartTime,
                ClassEndTime = x.Class.EndTime,
                ClassDay = x.Class.Day,
                ProfessorFullName = x.Class.ProfessorId.ToString(),
                CourseName = x.Class.Course.Name,
                SessionId = x.Id
            }).ToList();
        }

        public SessionQueryModel GetSessionById(long sessionId)
        {
            return _studyContext.Sessions.Select(x => new SessionQueryModel
            {
                Id = x.Id,
                Booklet = x.Booklet,
                ClassId = x.ClassId,
                Description = x.Description,
                IsActive = x.IsActive,
                Price = x.Price,
                Number = x.Number,
                IsPayed = x.IsPayed,
                Video = x.Video,
                Title = x.Title,
                SessionPictures = MapSessionPictures(x.SessionPictures)
            }).FirstOrDefault(x=>x.Id == sessionId);
        }

        private static List<SessionPictureQueryModel> MapSessionPictures(ICollection<SessionPicture> sessionPictures)
        {
            return sessionPictures.Select(x => new SessionPictureQueryModel
            {
                SessionId = x.SessionId,
                Picture = x.Picture,
                IsRemoved = x.IsRemoved
            }).Where(x => !x.IsRemoved).ToList();
        }
    }
}
