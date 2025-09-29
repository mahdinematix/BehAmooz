namespace _02_Query.Contracts.Message
{
    public interface IMessageQuery
    {
        List<MessageQueryModel> GetAdminMessages();
        List<MessageQueryModel> GetStudentMessages();
    }
}
