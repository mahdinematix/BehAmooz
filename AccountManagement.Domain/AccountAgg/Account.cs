using _01_Framework.Application;
using _01_Framework.Domain;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Domain.AccountAgg
{
    public class Account : EntityBase
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string NationalCode { get; private set; }
        public string Code { get; private set; }
        public int UniversityType { get; private set; }
        public int University { get; private set; }
        public int Major { get; private set; }
        public string NationalCardPicture { get; private set; }
        public int Status { get; private set; }
        public Role Role { get; set; }
        public long RoleId { get; private set; }

        public Account(string firstName, string lastName, string password, string email, string phoneNumber, string nationalCode, string code, int universityType, int university, int major, string nationalCardPicture, long roleId)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
            NationalCode = nationalCode;
            Code = code;
            UniversityType = universityType;
            University = university;
            Major = major;
            NationalCardPicture = nationalCardPicture;
            Status = Statuses.Waiting;
            RoleId = roleId;
        }

        public void Edit(string firstName, string lastName, string email, string phoneNumber, string nationalCode, string code, int universityType, int university, int major, string nationalCardPicture, long roleId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            NationalCode = nationalCode;
            Code = code;
            UniversityType = universityType;
            University = university;
            Major = major;
            NationalCardPicture = nationalCardPicture;
            RoleId = roleId;
        }

        public void Confirm()
        {
            Status = Statuses.Confirmed;
        }

        public void Reject()
        {
            Status = Statuses.Rejected;
        }

        public void ChangePassword(string password)
        {
            Password = password;
        }
    }
}
