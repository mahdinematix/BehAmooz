using _01_Framework.Domain;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Domain.CourseAgg
{
    public class Course : EntityBase 
    {
        public string Name { get; private set; }
        public int NumberOfUnit { get; private set; }
        public string CourseKind { get; private set; }
        public string Code { get; private set; }
        public string Major { get; private set; }
        public bool IsActive { get; private set; }
        public ICollection<Class> Classes { get; private set; }


        public Course(string name, int numberOfUnit, string courseKind, string code,string major)
        {
            Name = name;
            NumberOfUnit = numberOfUnit;
            CourseKind = courseKind;
            Code = code;
            Major = major;
            IsActive = true;
            Classes = new List<Class>();
        }

        public void Edit(string name, int numberOfUnit, string courseKind, string code, string major)
        {
            Name = name;
            NumberOfUnit = numberOfUnit;
            CourseKind = courseKind;
            Code = code;
            Major = major;
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
