using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.University;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class AccountRepository : RepositoryBase<long, Account> , IAccountRepository
    {
        private readonly AccountContext _context;
        private readonly IUniversityApplication _universityApplication;

        public AccountRepository(AccountContext context, IUniversityApplication universityApplication) : base(context)
        {
            _context = context;
            _universityApplication = universityApplication;
        }

        public EditAccount GetDetails(long id)
        {
            return _context.Accounts.Select(x => new EditAccount
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Major = x.MajorId,
                NationalCode = x.NationalCode,
                PhoneNumber = x.PhoneNumber,
                Code = x.Code,
                University = x.UniversityId,
                RoleId = x.RoleId,
                UniversityType = x.UniversityTypeId,
                EducationLevel = x.EducationLevel
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel, string currentAccountRole, long currentAccountUniversityId)
        {
            var query = _context.Accounts.Include(x=>x.Role).Select(x => new AccountViewModel
            {
                Id = x.Id,
                Email = x.Email,
                FullName = x.FirstName + " " + x.LastName,
                MajorId = x.MajorId,
                NationalCardPicture = x.NationalCardPicture,
                NationalCode = x.NationalCode,
                PhoneNumber = x.PhoneNumber,
                Code = x.Code,
                UniversityId = x.UniversityId,
                UniversityName = _universityApplication.GetNameBy(x.UniversityId),
                UniversityTypeId = x.UniversityTypeId,
                RoleId = x.RoleId,
                Role = x.Role.Name,
                CreationDate = x.CreationDate.ToFarsi(),
                Status = x.Status,
                EducationLevel = x.EducationLevel
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
            {
                query = query.Where(x => x.Email.Contains(searchModel.Email));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            {
                query = query.Where(x => x.FullName.Contains(searchModel.FullName));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.NationalCode))
            {
                query = query.Where(x => x.NationalCode.Contains(searchModel.NationalCode));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber.Contains(searchModel.PhoneNumber));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                query = query.Where(x => x.Code.Contains(searchModel.Code));
            }
            if (currentAccountRole == Roles.SuperAdministrator)
            {
                if (searchModel.UniversityTypeId > 0)
                {
                    query = query.Where(x => x.UniversityTypeId == searchModel.UniversityTypeId);
                }
                if (searchModel.UniversityId > 0)
                {
                    query = query.Where(x => x.UniversityId == searchModel.UniversityId);
                }
            }
            if (currentAccountRole == Roles.Administrator)
            {
                query = query.Where(x => x.UniversityId == currentAccountUniversityId);
            }

            if (searchModel.MajorId >0)
            {
                query = query.Where(x => x.MajorId == searchModel.MajorId);
            }

            if (searchModel.RoleId > 0)
            {
                query = query.Where(x => x.RoleId == searchModel.RoleId);
            }
            if (searchModel.Status > 0)
            {
                query = query.Where(x => x.Status == searchModel.Status);
            }
            if (searchModel.EducationLevel > 0)
            {
                query = query.Where(x => x.EducationLevel == searchModel.EducationLevel);
            }

            if (currentAccountRole==Roles.Administrator)
            {
                return query.Where(x => x.RoleId != long.Parse(Roles.Administrator)).Where(x => x.RoleId != long.Parse(Roles.SuperAdministrator)).OrderByDescending(x => x.Id).ToList();
            }
            return query.OrderByDescending(x => x.Id).ToList();
        }

        public Account GetByNationalCode(string nationalCode)
        {
            return _context.Accounts.FirstOrDefault(x => x.NationalCode == nationalCode);
        }

        public List<AccountViewModel> GetProfessors()
        {
            return _context.Accounts.Where(x => x.RoleId == long.Parse(Roles.Professor)).Where(x=>x.Status == Statuses.Confirmed).Select(x => new AccountViewModel
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName
            }).ToList();
        }

        public string GetProfessorById(long professorId)
        {
            var firstName = _context.Accounts.Where(x => x.RoleId == long.Parse(Roles.Professor))
                .FirstOrDefault(x => x.Id == professorId).FirstName;

            var lastName = _context.Accounts.Where(x => x.RoleId == long.Parse(Roles.Professor))
                .FirstOrDefault(x => x.Id == professorId).LastName;

            return firstName + " " + lastName;
        }

        public List<AccountViewModel> SearchInStudents(AccountSearchModel searchModel)
        {
            var query = _context.Accounts.Where(x=>x.RoleId == RolesId.Student).Select(x => new AccountViewModel
            {
                Id = x.Id,
                Email = x.Email,
                FullName = x.FirstName + " " + x.LastName,
                MajorId = x.MajorId,
                NationalCardPicture = x.NationalCardPicture,
                NationalCode = x.NationalCode,
                PhoneNumber = x.PhoneNumber,
                Code = x.Code,
                UniversityId = x.UniversityId,
                UniversityName = _universityApplication.GetNameBy(x.UniversityId),
                UniversityTypeId = x.UniversityTypeId,
                RoleId = x.RoleId,
                CreationDate = x.CreationDate.ToFarsi(),
                Status = x.Status,
                EducationLevel = x.EducationLevel
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
            {
                query = query.Where(x => x.Email.Contains(searchModel.Email));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            {
                query = query.Where(x => x.FullName.Contains(searchModel.FullName));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.NationalCode))
            {
                query = query.Where(x => x.NationalCode.Contains(searchModel.NationalCode));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber.Contains(searchModel.PhoneNumber));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                query = query.Where(x => x.Code.Contains(searchModel.Code));
            }
            if (searchModel.UniversityTypeId > 0)
            {
                query = query.Where(x => x.UniversityTypeId == searchModel.UniversityTypeId);
            }
            if (searchModel.UniversityId > 0)
            {
                query = query.Where(x => x.UniversityId == searchModel.UniversityId);
            }
            

            if (searchModel.MajorId > 0)
            {
                query = query.Where(x => x.MajorId == searchModel.MajorId);
            }

            if (searchModel.Status > 0)
            {
                query = query.Where(x => x.Status == searchModel.Status);
            }
            if (searchModel.EducationLevel > 0)
            {
                query = query.Where(x => x.EducationLevel == searchModel.EducationLevel);
            }

            return query.OrderByDescending(x => x.Id).ToList();
        }

        public List<AccountViewModel> SearchInCustomers(AccountSearchModel searchModel)
        {
            var query = _context.Accounts.Where(x => x.RoleId == RolesId.Student).Select(x => new AccountViewModel
            {
                Id = x.Id,
                Email = x.Email,
                FullName = x.FirstName + " " + x.LastName,
                MajorId = x.MajorId,
                NationalCardPicture = x.NationalCardPicture,
                NationalCode = x.NationalCode,
                PhoneNumber = x.PhoneNumber,
                Code = x.Code,
                UniversityId = x.UniversityId,
                UniversityName = _universityApplication.GetNameBy(x.UniversityId),
                UniversityTypeId = x.UniversityTypeId,
                RoleId = x.RoleId,
                CreationDate = x.CreationDate.ToFarsi(),
                Status = x.Status,
                EducationLevel = x.EducationLevel
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
            {
                query = query.Where(x => x.Email.Contains(searchModel.Email));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            {
                query = query.Where(x => x.FullName.Contains(searchModel.FullName));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.NationalCode))
            {
                query = query.Where(x => x.NationalCode.Contains(searchModel.NationalCode));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber.Contains(searchModel.PhoneNumber));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                query = query.Where(x => x.Code.Contains(searchModel.Code));
            }
            if (searchModel.UniversityId > 0)
            {
                query = query.Where(x => x.UniversityId == searchModel.UniversityId);
            }
            if (searchModel.UniversityTypeId > 0)
            {
                query = query.Where(x => x.UniversityTypeId == searchModel.UniversityTypeId);
            }

            if (searchModel.MajorId > 0)
            {
                query = query.Where(x => x.MajorId == searchModel.MajorId);
            }

            if (searchModel.Status > 0)
            {
                query = query.Where(x => x.Status == searchModel.Status);
            }
            if (searchModel.EducationLevel > 0)
            {
                query = query.Where(x => x.EducationLevel == searchModel.EducationLevel);
            }

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
