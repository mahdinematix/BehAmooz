using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Class
{
    public interface IClassApplication
    {
        OperationResult Create(CreateClass command, long currentAccountId);
        OperationResult Edit(EditClass command, long currentAccountId);
        OperationResult Activate(long id, long currentAccountId);
        OperationResult DeActivate(long id, long currentAccountId);
        EditClass GetDetails(long id);
        List<ClassViewModel> Search(ClassSearchModel searchModel, long courseId, long currentAccountId, string currentAccountRole);
        ClassViewModel GetClassById(long id);
        long GetTemplateIdByClassId(long classId);

    }
}
