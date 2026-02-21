namespace LogManagement.Application.Contracts.LogContracts
{
    public class LogSearchModel
    {
        public int Operation { get; set; }
        public int TargetType { get; set; }
        public string AccountRole { get; set; }
        public string AccountName { get; set; }
        public string AccountNationalCode { get; set; }
        public string Description { get; set; }
        public int UniversityTypeId { get; set; }
        public int UniversityId { get; set; }

    }
}
