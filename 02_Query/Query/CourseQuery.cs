using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Course;
using StudyManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class CourseQuery : ICourseQuery
    {
        private readonly StudyContext _studyContext;
        private readonly IAuthHelper _authHelper;

        public CourseQuery(StudyContext studyContext, IAuthHelper authHelper)
        {
            _studyContext = studyContext;
            _authHelper = authHelper;
        }

        public List<CourseQueryModel> Search(CourseSearchModel searchModel)
        {
            IQueryable<CourseQueryModel> query;
            if (_authHelper.CurrentAccountRole()==Roles.Student)
            {
                query = _studyContext.Courses.Where(x => x.IsActive)
                    .Where(x => x.Major == _authHelper.GetAccountInfo().MajorId)
                    .Where(x => x.University == _authHelper.GetAccountInfo().UniversityId)
                    .Where(x => x.EducationLevel == _authHelper.CurrentAccountEducationLevel()).Select(x =>
                        new CourseQueryModel
                        {
                            University = x.University,
                            Major = x.Major,
                            Code = x.Code,
                            IsActive = x.IsActive,
                            CourseKind = x.CourseKind,
                            Name = x.Name,
                            NumberOfUnit = x.NumberOfUnit,
                            Id = x.Id,
                            Price = x.Price,
                            EducationLevel = x.EducationLevel
                        });
            }
            else
            { 
                query = _studyContext.Courses.Where(x => x.IsActive)
                    .Where(x => x.University == _authHelper.GetAccountInfo().UniversityId)
                    .Select(x =>
                        new CourseQueryModel
                        {
                            University = x.University,
                            Major = x.Major,
                            Code = x.Code,
                            IsActive = x.IsActive,
                            CourseKind = x.CourseKind,
                            Name = x.Name,
                            NumberOfUnit = x.NumberOfUnit,
                            Id = x.Id,
                            Price = x.Price,
                            EducationLevel = x.EducationLevel
                        });
            }
            

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
            {
                query = query.Where(x => x.Name.Contains(searchModel.Name));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                query = query.Where(x => x.Code.Contains(searchModel.Code));
            }

            if (searchModel.CourseKind != "0" && searchModel.CourseKind != null)
            {
                query = query.Where(x => x.CourseKind == searchModel.CourseKind);
            }


            return query.ToList();
        }

        public string GetCourseNameById(long courseId)
        {
            return _studyContext.Courses.FirstOrDefault(x => x.Id == courseId).Name;
        }
    }
}
