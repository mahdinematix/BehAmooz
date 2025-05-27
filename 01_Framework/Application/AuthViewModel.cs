namespace _01_Framework.Application;

public class AuthViewModel
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public string Username { get; set; }
    public string Fullname { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Address { get; set; }
    public string Zipcode { get; set; }
    public string ProfilePhoto { get; set; }
    public List<int> Permissions { get; set; }

    public AuthViewModel()
    {
    }

    public AuthViewModel(long id, long roleId, string username, string fullname, string mobile, string email,string address,string zipcode,string profilePhoto, List<int> permissions)
    {
        Id = id;
        RoleId = roleId;
        Username = username;
        Fullname = fullname;
        Mobile = mobile;
        Email = email;
        Address = address;
        Zipcode = zipcode;
        ProfilePhoto = profilePhoto;
        Permissions = permissions;
    }
}