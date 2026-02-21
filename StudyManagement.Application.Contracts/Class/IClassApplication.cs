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
        List<ClassViewModel> Search(ClassSearchModel searchModel, long courseId);
        List<ClassViewModel> GetClasses(long classId);
        ClassViewModel GetClassById(long id);
        OperationResult Copy(CopyClass command, long currentAccountId);
        string GetClassCodeById(long id);
        List<ClassViewModel> GetClassesForCopy(long classId);
        string GetCourseNameByClassId(long classId);
        ClassInfoForCopy GetClassInfoByClassCode(string classCode);
    }
}
