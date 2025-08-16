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
        public int UniversityTypeId { get; private set; }
        public int UniversityId { get; private set; }
        public int MajorId { get; private set; }
        public string NationalCardPicture { get; private set; }
        public int Status { get; private set; }
        public Role Role { get; set; }
        public long RoleId { get; private set; }


        public Account(string firstName, string lastName, string password, string email, string phoneNumber, string nationalCode, string code, int universityTypeId, int universityId, int majorId, string nationalCardPicture, long roleId)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
            NationalCode = nationalCode;
            Code = code;
            UniversityTypeId = universityTypeId;
            UniversityId = universityId;
            MajorId = majorId;
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
            UniversityTypeId = universityType;
            UniversityId = university;
            MajorId = major;
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

        public void ChangeStatusToWaiting()
        {
            Status = Statuses.Waiting;
        }
    }
}
