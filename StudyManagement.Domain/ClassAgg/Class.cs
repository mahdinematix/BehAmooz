using _01_Framework.Domain;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Domain.ClassAgg
{
    public class Class : EntityBase
    {
        public string Code { get; private set; }
        public int Day { get; private set; }
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }
        public bool IsActive { get; private set; }
        public long ProfessorId { get; private set; }
        public long CourseId { get; private set; }
        public Course Course { get; private set; }
        public ICollection<Session> Sessions { get; set; }

        public Class(string code, string startTime, string endTime, long courseId,int day, long professorId)
        {
            Code = code;
            StartTime = startTime;
            EndTime = endTime;
            CourseId = courseId;
            IsActive = true;
            Day = day;
            ProfessorId = professorId;
            Sessions = new List<Session>();
        }

        public void Edit(string code, string startTime, string endTime, long courseId,int day, long professorId)
        {
            Code = code;
            StartTime = startTime;
            EndTime = endTime;
            CourseId = courseId;
            Day = day;
            ProfessorId = professorId;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void DeActivate()
        {
            IsActive = false;
        }
    }
}
