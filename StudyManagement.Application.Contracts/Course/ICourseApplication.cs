using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Course
{
    public interface ICourseApplication
    {
        OperationResult Create(CreateCourse command);
        OperationResult Edit(EditCourse command);
        OperationResult Activate(long id);
        OperationResult DeActivate(long id);
        List<CourseViewModel> Search(CourseSearchModel searchModel);
        EditCourse GetDetails(long id);
        List<CourseViewModel> GetCourses();
    }
}
