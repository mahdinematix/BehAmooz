namespace AccountManagement.Application.Contract.Account;

public class AccountSearchModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string NationalCode { get; set; }
    public int UniversityTypeId { get; set; }
    public int UniversityId { get; set; }
    public int MajorId { get; set; }
    public string Code { get; set; }
    public long RoleId { get; set; }
    public int Status { get; set; }
    public int EducationLevel { get; set; }
}