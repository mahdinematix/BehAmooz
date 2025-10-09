using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class AccountRepository : RepositoryBase<long, Account> , IAccountRepository
    {
        private readonly AccountContext _context;

        public AccountRepository(AccountContext context) : base(context)
        {
            _context = context;
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
                UniversityType = x.UniversityTypeId
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
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
                UniversityTypeId = x.UniversityTypeId,
                RoleId = x.RoleId,
                Role = x.Role.Name,
                CreationDate = x.CreationDate.ToFarsi(),
                Status = x.Status
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
            var query = _context.Accounts.Where(x=>x.RoleId == RolesInt.Student).Select(x => new AccountViewModel
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
                UniversityTypeId = x.UniversityTypeId,
                RoleId = x.RoleId,
                CreationDate = x.CreationDate.ToFarsi(),
                Status = x.Status
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

            return query.OrderByDescending(x => x.Id).ToList();
        }

        public List<AccountViewModel> SearchInCustomers(AccountSearchModel searchModel)
        {
            var query = _context.Accounts.Where(x => x.RoleId == RolesInt.Student).Select(x => new AccountViewModel
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
                UniversityTypeId = x.UniversityTypeId,
                RoleId = x.RoleId,
                CreationDate = x.CreationDate.ToFarsi(),
                Status = x.Status
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

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
