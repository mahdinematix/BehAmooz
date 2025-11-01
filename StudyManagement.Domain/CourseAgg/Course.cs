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
        public int Major { get; private set; }
        public int UniversityType { get; set; }
        public int University { get; private set; }
        public int Price { get; private set; }
        public int EducationLevel { get; set; }
        public bool IsActive { get; private set; }
        public ICollection<Class> Classes { get; private set; }


        public Course(string name, int numberOfUnit, string courseKind, string code,int major,int universityType, int university, int price,int educationLevel)
        {
            Name = name;
            NumberOfUnit = numberOfUnit;
            CourseKind = courseKind;
            Code = code;
            Major = major;
            UniversityType = universityType;
            University = university;
            IsActive = true;
            Classes = new List<Class>();
            Price = price;
            EducationLevel = educationLevel;
        }

        public void Edit(string name, int numberOfUnit, string courseKind, string code, int major,int universityType, int university, int price, int educationLevel)
        {
            Name = name;
            NumberOfUnit = numberOfUnit;
            CourseKind = courseKind;
            Code = code;
            Major = major;
            UniversityType = universityType;
            University = university;
            Price = price;
            EducationLevel = educationLevel;
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
