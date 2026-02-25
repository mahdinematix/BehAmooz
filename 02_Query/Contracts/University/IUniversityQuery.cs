namespace _02_Query.Contracts.University
{
    public interface IUniversityQuery
    {
        UniversityQueryModel GetById(long id);
    }
}
