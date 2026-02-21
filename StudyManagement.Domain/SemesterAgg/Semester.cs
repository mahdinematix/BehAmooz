using _01_Framework.Domain;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Domain.UniversityAgg;

namespace StudyManagement.Domain.SemesterAgg
{
    public class Semester : EntityBase
    {
        public int MidYear { get; private set; }
        public int Year { get; private set; }
        public int Code { get; private set; }
        public bool IsCurrent { get; private set; }
        public long UniversityId { get; private set; }
        public University University { get; private set; }
        public List<Course> Courses { get; private set; }

        public Semester(int midYear, int year, long universityId)
        {
            MidYear = midYear;
            Year = year;
            Code = GenerateCode(year, midYear);
            UniversityId = universityId;
            IsCurrent = false;
            Courses = new List<Course>();
        }

        public void SetAsCurrent()
        {
            IsCurrent = true;
        }

        public void UnsetCurrent()
        {
            IsCurrent = false;
        }

        private static int GenerateCode(int year, int midYear)
        {
            var yearPart = year.ToString().Substring(1);
            return int.Parse($"{yearPart}{midYear}");
        }

    }
}
