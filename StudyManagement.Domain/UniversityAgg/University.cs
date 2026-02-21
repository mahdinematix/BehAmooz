using _01_Framework.Domain;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Domain.SemesterAgg;

namespace StudyManagement.Domain.UniversityAgg
{
    public class University : EntityBase
    {
        public string Name { get; private set; }
        public int Type { get; private set; }
        public long CurrentSemesterId { get; private set; }
        public bool IsActive { get; private set; }
        public List<Semester> Semesters { get; private set; }
        public List<Course> Courses { get; private set; }

        public University(string name, int type)
        {
            Name = name;
            Type = type;
            CurrentSemesterId = 0;
            IsActive = false;
            Semesters = new List<Semester>();
            Courses = new List<Course>();
        }
        public void Edit(string name, int type)
        {
            Name = name;
            Type = type;
        }

        public void SetCurrentSemester(long semesterId)
        {
            CurrentSemesterId = semesterId;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

    }
}
