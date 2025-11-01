namespace AccountManagement.Application.Contract.Account;

public class AccountViewModel
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string NationalCode { get; set; }
    public int UniversityTypeId { get; set; }
    public int UniversityId { get; set; }
    public int MajorId { get; set; }
    public string NationalCardPicture { get; set; }
    public string Code { get; set; }
    public long RoleId { get; set; }
    public string Role { get; set; }
    public string CreationDate { get; set; }
    public int Status { get; set; }
    public int EducationLevel { get; set; }
}