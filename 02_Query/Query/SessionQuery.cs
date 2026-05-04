using _02_Query.Contracts.Session;
using _02_Query.Contracts.SessionPicture;
using AccountManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.Order;
using StudyManagement.Domain.SessionPictureAgg;
using StudyManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class SessionQuery : ISessionQuery
    {

        private readonly StudyContext _studyContext;
        private readonly AccountContext _accountContext;

        public SessionQuery(StudyContext studyContext, AccountContext accountContext)
        {
            _studyContext = studyContext;
            _accountContext = accountContext;
        }

        public List<CartItem> GetItemsByClassId(long classId)
        {
           
            var templateId = _studyContext.Classes
                .Where(c => c.Id == classId && c.IsActive)
                .Select(c => c.ClassTemplateId)
                .FirstOrDefault();

            if (templateId == 0)
                return new List<CartItem>();

            
            var classInfo = _studyContext.Classes
                .Where(c => c.Id == classId)
                .Select(c => new { c.StartTime, c.EndTime, c.Day })
                .FirstOrDefault();

            if (classInfo == null)
                return new List<CartItem>();

            
            var templateInfo = _studyContext.ClassTemplates
                .Where(t => t.Id == templateId)
                .Select(t => new { t.CourseId, t.ProfessorId })
                .FirstOrDefault();

            if (templateInfo == null)
                return new List<CartItem>();

            
            var courseInfo = _studyContext.Courses
                .Where(c => c.Id == templateInfo.CourseId)
                .Select(c => new { c.Name, c.Price })
                .FirstOrDefault();

            if (courseInfo == null)
                return new List<CartItem>();

            
            var professorName = _accountContext.Accounts
                .Where(a => a.Id == templateInfo.ProfessorId)
                .Select(a => a.FirstName + " " + a.LastName)
                .FirstOrDefault() ?? "";

           
            return _studyContext.Sessions
                .Where(s => s.IsActive && s.ClassTemplateId == templateId)
                .Select(s => new CartItem
                {
                    Id = s.Id,
                    SessionId = s.Id,
                    SessionNumber = s.Number,
                    ClassStartTime = classInfo.StartTime,
                    ClassEndTime = classInfo.EndTime,
                    ClassDay = classInfo.Day,
                    ProfessorFullName = professorName,
                    CourseName = courseInfo.Name,
                    SessionPrice = courseInfo.Price
                })
                .OrderBy(x => x.SessionNumber)
                .ToList();
        }

        public SessionQueryModel GetSessionById(long sessionId)
        {
            return _studyContext.Sessions.Select(x => new SessionQueryModel
            {
                Id = x.Id,
                Booklet = x.Booklet,
                ClassTemplateId = x.ClassTemplateId,
                Description = x.Description,
                IsActive = x.IsActive,
                Number = x.Number,
                Video = x.Video,
                Title = x.Title,
                SessionPictures = MapSessionPictures(x.SessionPictures)
            }).FirstOrDefault(x=>x.Id == sessionId);
        }



        private static List<SessionPictureQueryModel> MapSessionPictures(ICollection<SessionPicture> sessionPictures)
        {
            return sessionPictures.Where(x=>!x.IsRemoved).Select(x => new SessionPictureQueryModel
            {
                SessionId = x.SessionId,
                Picture = x.Picture,
                IsRemoved = x.IsRemoved
            }).Where(x => !x.IsRemoved).ToList();
        }
    }
}
