namespace LogManagement.Application.Contracts.LogContracts
{
    public class LogViewModel
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string AccountRole { get; set; }
        public string AccountName { get; set; }
        public string AccountNationalCode { get; set; }
        public string Operation { get; set; }
        public string TargetType { get; set; }
        public long TargetId { get; set; }
        public string TargetName { get; set; }
        public string Description { get; set; }
        public string CreationDate { get; set; }
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
    }
}