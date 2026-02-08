using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using LogManagement.Application.Contracts.Log;
using LogManagement.Domain.LogAgg;
using StudyManagement.Infrastructure.EFCore;

namespace LogManagement.Infrastructure.EFCore.Repository
{
    public class LogRepository : RepositoryBase<long, Log>, ILogRepository
    {
        private readonly LogContext _context;
        private readonly AccountContext _accountContext;
        private readonly StudyContext _studyContext;
        public LogRepository(LogContext context, AccountContext accountContext, StudyContext studyContext) : base(context)
        {
            _context = context;
            _accountContext = accountContext;
            _studyContext = studyContext;
        }

        public List<LogViewModel> Search(LogSearchModel searchModel)
        {

            var accounts = _accountContext.Accounts
                .Select(a => new
                {
                    a.Id,
                    FullName = a.FirstName + " " + a.LastName,
                    a.NationalCode,
                    a.RoleId
                })
                .ToList();
            var roles = _accountContext.Roles
                .Select(r => new
                {
                    r.Id,
                    r.Name
                })
                .ToList();


            var query = _context.Logs.AsQueryable();

            if(searchModel.Operation > 0)
                query = query.Where(x => x.Operation == searchModel.Operation);

            if (searchModel.TargetType > 0)
                query = query.Where(x => x.TargetType == searchModel.TargetType);

            if (!string.IsNullOrWhiteSpace(searchModel.Description))
                query = query.Where(x => x.Description.Contains(searchModel.Description));

            var logs = query
                .OrderByDescending(x => x.Id)
                .ToList();

            var result = logs.Select(x =>
            {
                var account = accounts.FirstOrDefault(a => a.Id == x.AccountId);
                var roleName = roles.FirstOrDefault(r => r.Id == account?.RoleId)?.Name;


                return new LogViewModel
                {
                    Id = x.Id,
                    AccountName = account?.FullName,
                    AccountNationalCode = account?.NationalCode,
                    AccountRole = roleName,
                    Operation = Operations.GetOperationBy(x.Operation),
                    CreationDate = x.CreationDate.ToFarsi(),
                    Description = x.Description,
                    TargetType = TargetTypes.GetTargetTypeBy(x.TargetType),
                };
            }).ToList();

            if (!string.IsNullOrWhiteSpace(searchModel.AccountName))
                result = result.Where(x => x.AccountName.Contains(searchModel.AccountName)).ToList();

            if (!string.IsNullOrWhiteSpace(searchModel.AccountNationalCode))
                result = result.Where(x => x.AccountNationalCode.Contains(searchModel.AccountNationalCode)).ToList();

            if (!string.IsNullOrWhiteSpace(searchModel.AccountRole) && searchModel.AccountRole != "0")
                result = result.Where(x => x.AccountRole==searchModel.AccountRole).ToList();

            return result;
        }

        public List<LogViewModel> GetCourseLogsById(long id)
        {
            var accounts = _accountContext.Accounts
                .Select(a => new
                {
                    a.Id,
                    FullName = a.FirstName + " " + a.LastName,
                    a.NationalCode,
                    a.RoleId
                })
                .ToList();
            var roles = _accountContext.Roles
                .Select(r => new
                {
                    r.Id,
                    r.Name
                })
                .ToList();

            var courses = _studyContext.Courses
                .Select(x => new { x.Id, x.Name })
                .ToList();

            var query = _context.Logs.AsQueryable();

            var logs = query.Where(x => x.TargetId == id)
                .OrderByDescending(x => x.Id)
                .ToList();

            var result = logs.Select(x =>
            {
                var account = accounts.FirstOrDefault(a => a.Id == x.AccountId);
                var roleName = roles.FirstOrDefault(r => r.Id == account?.RoleId)?.Name;


                string courseName = courses.FirstOrDefault(c => c.Id == x.TargetId)?.Name;

                return new LogViewModel
                {
                    AccountId = x.AccountId,
                    AccountName = account?.FullName,
                    AccountNationalCode = account?.NationalCode,
                    AccountRole = roleName,
                    Operation = Operations.GetOperationBy(x.Operation),
                    CreationDate = x.CreationDate.ToFarsi(),
                    Description = x.Description,
                    TargetId = x.TargetId,
                    TargetType = TargetTypes.GetTargetTypeBy(TargetTypes.Course),
                    TargetName = courseName
                };
            }).ToList();

            return result;
        }

        public List<LogViewModel> GetClassLogsById(long id)
        {
            var accounts = _accountContext.Accounts
                .Select(a => new
                {
                    a.Id,
                    FullName = a.FirstName + " " + a.LastName,
                    a.NationalCode,
                    a.RoleId
                })
                .ToList();
            var roles = _accountContext.Roles
                .Select(r => new
                {
                    r.Id,
                    r.Name
                })
                .ToList();

            var classes = _studyContext.Classes
                .Select(x => new { x.Id, x.Code })
                .ToList();

            var query = _context.Logs.AsQueryable();

            var logs = query.Where(x => x.TargetId == id)
                .OrderByDescending(x => x.Id)
                .ToList();

            var result = logs.Select(x =>
            {
                var account = accounts.FirstOrDefault(a => a.Id == x.AccountId);
                var roleName = roles.FirstOrDefault(r => r.Id == account?.RoleId)?.Name;


                string classCode = classes.FirstOrDefault(c => c.Id == x.TargetId)?.Code;

                return new LogViewModel
                {
                    AccountId = x.AccountId,
                    AccountName = account?.FullName,
                    AccountNationalCode = account?.NationalCode,
                    AccountRole = roleName,
                    Operation = Operations.GetOperationBy(x.Operation),
                    CreationDate = x.CreationDate.ToFarsi(),
                    Description = x.Description,
                    TargetId = x.TargetId,
                    TargetType = TargetTypes.GetTargetTypeBy(TargetTypes.Class),
                    TargetName = classCode
                };
            }).ToList();

            return result;
        }

        public List<LogViewModel> GetSessionLogsById(long id)
        {
            var accounts = _accountContext.Accounts
                .Select(a => new
                {
                    a.Id,
                    FullName = a.FirstName + " " + a.LastName,
                    a.NationalCode,
                    a.RoleId
                })
                .ToList();
            var roles = _accountContext.Roles
                .Select(r => new
                {
                    r.Id,
                    r.Name
                })
                .ToList();

            var sessions = _studyContext.Sessions
                .Select(x => new { x.Id, x.Number })
                .ToList();

            var query = _context.Logs.AsQueryable();

            var logs = query.Where(x => x.TargetId == id)
                .OrderByDescending(x => x.Id)
                .ToList();

            var result = logs.Select(x =>
            {
                var account = accounts.FirstOrDefault(a => a.Id == x.AccountId);
                var roleName = roles.FirstOrDefault(r => r.Id == account?.RoleId)?.Name;


                string sessionNumber = sessions.FirstOrDefault(c => c.Id == x.TargetId)?.Number.ToString();

                return new LogViewModel
                {
                    AccountId = x.AccountId,
                    AccountName = account?.FullName,
                    AccountNationalCode = account?.NationalCode,
                    AccountRole = roleName,
                    Operation = Operations.GetOperationBy(x.Operation),
                    CreationDate = x.CreationDate.ToFarsi(),
                    Description = x.Description,
                    TargetId = x.TargetId,
                    TargetType = TargetTypes.GetTargetTypeBy(TargetTypes.Session),
                    TargetName = sessionNumber
                };
            }).ToList();

            return result;
        }

    }
}
