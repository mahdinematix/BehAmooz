namespace _01_Framework.Application;

public class AuthViewModel
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public string Fullname { get; set; }
    public string NationalCode { get; set; }
    public string Code { get; set; }
    public int UniversityTypeId { get; set; }
    public int UniversityId { get; set; }
    public int MajorId { get; set; }
    public string NationalCardPicture { get; set; }
    public int Status { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
    public int EducationLevel { get; set; }
    public List<int> Permissions { get; set; }


    public AuthViewModel()
    {
    }

    public AuthViewModel(long id, long roleId, string fullname, string nationalCode, string code, int universityTypeId, int universityId, int majorId, string nationalCardPicture, int status, string mobile, string email, List<int> permissions,string password, int educationLevel)
    {
        Id = id;
        RoleId = roleId;
        Fullname = fullname;
        NationalCode = nationalCode;
        Code = code;
        UniversityTypeId = universityTypeId;
        UniversityId = universityId;
        MajorId = majorId;
        NationalCardPicture = nationalCardPicture;
        Status = status;
        Mobile = mobile;
        Email = email;
        Permissions = permissions;
        Password = password;
        EducationLevel = educationLevel;
    }
}