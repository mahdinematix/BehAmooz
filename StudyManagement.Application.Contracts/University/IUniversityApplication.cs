using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.University
{
    public interface IUniversityApplication
    {
        OperationResult Define(DefineUniversity command);
        OperationResult Edit(EditUniversity command);
        OperationResult Activate(long id);
        OperationResult Deactivate(long id);
        List<UniversityViewModel> Search(UniversitySearchModel searchModel);
        EditUniversity GetDetails(long id);
        List<UniversityViewModel> GetUniversitiesByType(int typeId);
        OperationResult SetCurrentSemesterId(long id, long semesterId);
        long GetCurrentSemesterId(long id);
        string GetNameBy(long id);
        List<UniversityViewModel> GetActiveUniversities();
        List<UniversityViewModel> GetActiveUniversitiesByTypeId(int typeId);
    }
}
