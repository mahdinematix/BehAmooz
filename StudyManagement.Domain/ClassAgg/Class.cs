using _01_Framework.Domain;

namespace StudyManagement.Domain.ClassAgg
{
    public class Class : EntityBase
    {
        public string Code { get; private set; }
        public int Day { get; private set; }
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }
        public bool IsActive { get; private set; }
        public long ClassTemplateId { get; private set; }
        public ClassTemplate Template { get; private set; }

        public Class(string code, string startTime, string endTime, int day, long classTemplateId)
        {
            Code = code;
            StartTime = startTime;
            EndTime = endTime;
            Day = day;
            ClassTemplateId = classTemplateId;
            IsActive = true;
        }

        public void Edit(string code, string startTime, string endTime, int day, long classTemplateId)
        {
            Code = code;
            StartTime = startTime;
            EndTime = endTime;
            Day = day;
            ClassTemplateId = classTemplateId;
        }

        public void Activate() => IsActive = true;
        public void DeActivate() => IsActive = false;
    }
}
