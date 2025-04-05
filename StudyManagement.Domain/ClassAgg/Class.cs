using _01_Framework.Domain;
using StudyManagement.Domain.CourseAgg;

namespace StudyManagement.Domain.ClassAgg
{
    public class Class : EntityBase
    {
        public string Code { get; private set; }
        public string Day { get; private set; }
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }
        public bool IsActive { get; private set; }
        public long CourseId { get; private set; }
        public Course Course { get; private set; }

        public Class(string code, string startTime, string endTime, long courseId,string day)
        {
            Code = code;
            StartTime = startTime;
            EndTime = endTime;
            CourseId = courseId;
            IsActive = true;
            Day = day;
        }

        public void Edit(string code, string startTime, string endTime, long courseId,string day)
        {
            Code = code;
            StartTime = startTime;
            EndTime = endTime;
            CourseId = courseId;
            Day = day;
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
