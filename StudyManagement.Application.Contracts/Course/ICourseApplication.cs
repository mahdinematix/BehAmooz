using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Course
{
    public interface ICourseApplication
    {
        OperationResult Create(CreateCourse command, long currentAccountId);
        OperationResult Edit(EditCourse command, long currentAccountId);
        OperationResult Activate(long id, long currentAccountId);
        OperationResult DeActivate(long id, long currentAccountId);
        List<CourseViewModel> Search(CourseSearchModel searchModel, long universityId, long currentAccountId, string currentAccountRole);
        EditCourse GetDetails(long id);
        List<CourseViewModel> GetCourses();
        CourseViewModel GetById(long courseId);
    }
}
