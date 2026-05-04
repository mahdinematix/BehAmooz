using _02_Query.Contracts.Customer;
using AccountManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.University;
using StudyManagement.Infrastructure.EFCore;

namespace _02_Query.Query;

public class CustomerQuery : ICustomerQuery
{
    private readonly StudyContext _studyContext;
    private readonly AccountContext _accountContext;
    private readonly IUniversityApplication _universityApplication;

    public CustomerQuery(StudyContext studyContext, AccountContext accountContext, IUniversityApplication universityApplication)
    {
        _studyContext = studyContext;
        _accountContext = accountContext;
        _universityApplication = universityApplication;
    }

    public List<CustomerQueryModel> GetCustomersByProfessorId(CustomerSearchModel searchModel, long professorId)
    {

        var classes = (
            from c in _studyContext.Classes
            join t in _studyContext.ClassTemplates on c.ClassTemplateId equals t.Id
            join course in _studyContext.Courses on t.CourseId equals course.Id
            where t.ProfessorId == professorId && c.IsActive
            select new
            {
                ClassId = c.Id,
                c.Code,
                c.Day,
                c.StartTime,
                c.EndTime,
                CourseName = course.Name
            }
        ).ToList();

        if (classes.Count == 0)
            return new List<CustomerQueryModel>();

        var classIds = classes.Select(x => x.ClassId).ToList();

        var orderItemsForClasses = _studyContext.Orders
            .Where(o => o.IsPayed)
            .SelectMany(o => o.Items
                .Where(i => classIds.Contains(i.ClassId))
                .Select(i => new
                {
                    i.ClassId,
                    i.SessionNumber,
                    i.SessionPrice,
                    o.AccountId
                }))
            .ToList();

        if (orderItemsForClasses.Count == 0)
            return new List<CustomerQueryModel>();


        var accountIds = orderItemsForClasses.Select(x => x.AccountId).Distinct().ToList();

        var accounts = _accountContext.Accounts
            .Where(a => accountIds.Contains(a.Id))
            .Select(a => new
            {
                a.Id,
                a.FirstName,
                a.LastName,
                a.Code,
                a.UniversityId,
                a.EducationLevel
            })
            .ToList();

        var customers = new List<CustomerQueryModel>();


        var groupedItems = orderItemsForClasses
            .GroupBy(i => new { i.AccountId, i.ClassId })
            .ToList();

        foreach (var group in groupedItems)
        {
            var account = accounts.FirstOrDefault(a => a.Id == group.Key.AccountId);
            var classInfo = classes.FirstOrDefault(c => c.ClassId == group.Key.ClassId);

            if (account == null || classInfo == null)
                continue;

            var sessionCounts = new Dictionary<int, int>();
            for (int i = 1; i <= 16; i++)
                sessionCounts[i] = group.Count(it => it.SessionNumber == i);

            int totalSessions = sessionCounts.Values.Sum();
            long sessionPrice = group.FirstOrDefault()?.SessionPrice ?? 0;
            long totalAmount = totalSessions * sessionPrice;

            int organShare = (int)(totalAmount * 0.25);
            int tax = (int)(totalAmount * 0.10);
            long professorShare = totalAmount - organShare - tax;

            customers.Add(new CustomerQueryModel
            {
                AccountId = account.Id,
                FullName = $"{account.FirstName} {account.LastName}",
                Code = account.Code,
                UniversityId = account.UniversityId,
                UniversityName = _universityApplication.GetNameBy(account.UniversityId),

                CourseName = classInfo.CourseName,
                ClassCode = classInfo.Code,
                ClassDay = classInfo.Day,
                ClassStartTime = classInfo.StartTime,
                ClassEndTime = classInfo.EndTime,

                SessionCounts = sessionCounts,
                TotalSessions = totalSessions,
                SessionPrice = sessionPrice,
                TotalAmount = totalAmount,
                OrganShare = organShare,
                Tax = tax,
                ProfessorShare = professorShare,
                EducationLevel = account.EducationLevel
            });
        }


        if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            customers = customers.Where(x => x.FullName.Contains(searchModel.FullName)).ToList();

        if (!string.IsNullOrWhiteSpace(searchModel.ClassCode))
            customers = customers.Where(x => x.ClassCode.Contains(searchModel.ClassCode)).ToList();

        if (!string.IsNullOrWhiteSpace(searchModel.Code))
            customers = customers.Where(x => x.Code.Contains(searchModel.Code)).ToList();

        if (!string.IsNullOrWhiteSpace(searchModel.CourseName))
            customers = customers.Where(x => x.CourseName.Contains(searchModel.CourseName)).ToList();

        if (!string.IsNullOrWhiteSpace(searchModel.ClassStartTime) && searchModel.ClassStartTime != "0")
            customers = customers.Where(x => x.ClassStartTime == searchModel.ClassStartTime).ToList();

        if (searchModel.ClassDay > 0)
            customers = customers.Where(x => x.ClassDay == searchModel.ClassDay).ToList();

        if (searchModel.UniversityId > 0)
            customers = customers.Where(x => x.UniversityId == searchModel.UniversityId).ToList();

        if (searchModel.EducationLevel > 0)
            customers = customers.Where(x => x.EducationLevel == searchModel.EducationLevel).ToList();

        return customers;
    }
}
