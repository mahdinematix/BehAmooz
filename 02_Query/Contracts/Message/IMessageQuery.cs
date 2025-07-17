namespace _02_Query.Contracts.Message
{
    public interface IMessageQuery
    {
        List<MessageQueryModel> GetProfessorMessages();
        List<MessageQueryModel> GetStudentMessages();
    }
}
