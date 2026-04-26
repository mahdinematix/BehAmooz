using _01_Framework.Domain;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Domain.ClassAgg
{
    public class ClassTemplate : EntityBase
    {
        public long ProfessorId { get; private set; }
        public long CourseId { get; private set; }         
        public Course Course { get; private set; }         
        public List<Session> Sessions { get; private set; }
        public List<Class> Classes { get; private set; }

        public ClassTemplate(long courseId, long professorId)
        {
            CourseId = courseId;
            ProfessorId = professorId;
            Sessions = new List<Session>();
            Classes = new List<Class>();
        }

    }
}
