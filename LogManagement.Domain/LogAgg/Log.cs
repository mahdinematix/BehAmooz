using _01_Framework.Domain;

namespace LogManagement.Domain.LogAgg
{
    public class Log : EntityBase
    {
        public long AccountId { get; private set; }
        public int Operation { get; private set; }
        public int TargetType { get; private set; }
        public long TargetId { get; private set; }
        public string Description { get; private set; }

        public Log(long accountId, int operation, int targetType, long targetId, string description="")
        {
            AccountId = accountId;
            Operation = operation;
            TargetType = targetType;
            TargetId = targetId;
            Description = description;
        }
    }
}
