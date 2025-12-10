using System.Collections;
using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Class
{
    public interface IClassApplication
    {
        OperationResult Create(CreateClass command);
        OperationResult Edit(EditClass command);
        OperationResult Activate(long id);
        OperationResult DeActivate(long id);
        EditClass GetDetails(long id);
        List<ClassViewModel> Search(ClassSearchModel searchModel, long courseId);
        List<ClassViewModel> GetClasses();
        ClassViewModel GetClassById(long id);
        OperationResult Copy(CopyClass command);
        string GetClassCodeById(long id);
        List<ClassViewModel> GetClassesForCopy(long classId);
        
    }
}
