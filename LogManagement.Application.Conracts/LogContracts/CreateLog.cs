namespace LogManagement.Application.Contracts.LogContracts
{
    public class CreateLog
    {
        public long AccountId { get; set; }
        public int Operation { get; set; }
        public int TargetType { get; set; }
        public long TargetId { get; set; }
        public string Description { get; set; }
    }
}
