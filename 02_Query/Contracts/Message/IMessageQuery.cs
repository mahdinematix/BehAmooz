namespace _02_Query.Contracts.Message
{
    public interface IMessageQuery
    {
        List<MessageQueryModel> GetAdminMessages(long currentAccountUniversity);
        List<MessageQueryModel> GetStudentMessages(long currentAccountUniversity);
        List<MessageQueryModel> GetProfessorMessages(long currentAccountUniversity);
    }
}
