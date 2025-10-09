namespace _02_Query.Contracts.Customer
{
    public interface ICustomerQuery
    {
        List<CustomerQueryModel> GetCustomersByProfessorId(CustomerSearchModel searchModel, long professorId);
    }
}
