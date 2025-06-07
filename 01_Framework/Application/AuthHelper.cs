using System.Security.Claims;

using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace _01_Framework.Application
{
    public class AuthHelper : IAuthHelper
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public AuthHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        public void Signin(AuthViewModel account)
        {
            var permissions = JsonConvert.SerializeObject(account.Permissions);
            var claims = new List<Claim>
            {
                new Claim("AccountId", account.Id.ToString()),
                new Claim(ClaimTypes.Name,account.Fullname),
                new Claim(ClaimTypes.Role,account.RoleId.ToString()),
                new Claim(ClaimTypes.MobilePhone,account.Mobile),
                new Claim(ClaimTypes.Email,account.Email),
                new Claim("UniversityId",account.UniversityId.ToString()),
                new Claim("UniversityTypeId",account.UniversityTypeId.ToString()),
                new Claim("MajorId",account.MajorId.ToString()),
                new Claim("Status",account.Status.ToString()),
                new Claim("NationalCardPicture",account.NationalCardPicture),
                new Claim("NationalCode",account.NationalCode),
                new Claim("Code",account.Code),
                new Claim("permissions",permissions)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(2)
            };

            _contextAccessor.HttpContext.SignInAsync
            (
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }

        public void SignOut()
        {
            _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string CurrentAccountRole()
        {
            if (IsAuthenticated())
                return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;


            return null;
        }

        public AuthViewModel GetAccountInfo()
        {
            var result = new AuthViewModel();
            if (!IsAuthenticated())
            {
                return result;
            }

            var claims = _contextAccessor.HttpContext.User.Claims.ToList();

            result.Id = long.Parse(claims.FirstOrDefault(x => x.Type == "AccountId").Value);
            result.Fullname = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            result.Mobile = claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone).Value;
            result.Email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            result.RoleId = long.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);
            result.UniversityId = int.Parse(claims.FirstOrDefault(x => x.Type == "UniversityId").Value);
            result.UniversityTypeId = int.Parse(claims.FirstOrDefault(x => x.Type == "UniversityTypeId").Value);
            result.MajorId = int.Parse(claims.FirstOrDefault(x => x.Type == "MajorId").Value);
            result.Status = int.Parse(claims.FirstOrDefault(x => x.Type == "Status").Value);
            result.NationalCode = claims.FirstOrDefault(x => x.Type == "NationalCode").Value;
            result.Code = claims.FirstOrDefault(x => x.Type == "Code").Value;
            result.NationalCardPicture = claims.FirstOrDefault(x => x.Type == "NationalCardPicture").Value;
            result.Role = Roles.GetRoleBy(result.RoleId);

            return result;
        }

        public List<int> GetPermissions()
        {
            if (!IsAuthenticated())
            {
                return new List<int>();
            }
            var permissions = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "permissions").Value;

            return JsonConvert.DeserializeObject<List<int>>(permissions);
        }

        public long CurrentAccountId()
        {
            if (IsAuthenticated())
                return long.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "AccountId").Value);


            return 0;
        }

        public int CurrentAccountStatus()
        {
            if (IsAuthenticated())
                return int.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Status").Value);


            return 0;
        }
        

        

    }
}
