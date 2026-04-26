using _01_Framework.Domain;
using StudyManagement.Domain.ClassAgg;
using StudyManagement.Domain.SemesterAgg;
using StudyManagement.Domain.UniversityAgg;

namespace StudyManagement.Domain.CourseAgg
{
    public class Course : EntityBase 
    {
        public string Name { get; private set; }
        public int NumberOfUnit { get; private set; }
        public string CourseKind { get; private set; }
        public string Code { get; private set; }
        public int Major { get; private set; }
        public int Price { get; private set; }
        public int EducationLevel { get; set; }
        public long SemesterId { get; private set; }
        public long UniversityId { get; private set; }
        public bool IsActive { get; private set; }
        public List<ClassTemplate> ClassTemplates { get; private set; }
        public Semester Semester { get; private set; }
        public University University { get; private set; }


        public Course(string name, int numberOfUnit, string courseKind, string code,int major, int price,int educationLevel,long semesterId, long universityId)
        {
            Name = name;
            NumberOfUnit = numberOfUnit;
            CourseKind = courseKind;
            Code = code;
            Major = major;
            IsActive = true;
            Price = price;
            EducationLevel = educationLevel;
            SemesterId = semesterId;
            UniversityId = universityId;
            ClassTemplates = new List<ClassTemplate>();
        }

        public void Edit(string name, int numberOfUnit, string courseKind, string code, int major, int price, int educationLevel, long semesterId)
        {
            Name = name;
            NumberOfUnit = numberOfUnit;
            CourseKind = courseKind;
            Code = code;
            Major = major;
            Price = price;
            EducationLevel = educationLevel;
            SemesterId = semesterId;
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
