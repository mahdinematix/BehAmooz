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
                new Claim("Username",account.Username),
                new Claim(ClaimTypes.Role,account.RoleId.ToString()),
                new Claim(ClaimTypes.MobilePhone,account.Mobile),
                new Claim(ClaimTypes.Email,account.Email),
                new Claim(ClaimTypes.StreetAddress,account.Address),
                new Claim(ClaimTypes.PostalCode,account.Zipcode),
                new Claim(ClaimTypes.Uri,account.ProfilePhoto),
                new Claim("permissions",permissions),
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
            result.Username = claims.FirstOrDefault(x => x.Type == "Username").Value;
            result.Mobile = claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone).Value;
            result.Email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            result.RoleId = long.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);
            result.Address = claims.FirstOrDefault(x => x.Type == ClaimTypes.StreetAddress).Value;
            result.Zipcode = claims.FirstOrDefault(x => x.Type == ClaimTypes.PostalCode).Value;
            result.ProfilePhoto = claims.FirstOrDefault(x => x.Type == ClaimTypes.Uri).Value;
            result.Role = Roles.GetRoleBy(result.RoleId);

            return result;
        }

        public List<int> GetPermissions()
        {
            if (!IsAuthenticated())
            {
                return new List<int>();
            }
            var permissions = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "permissions")?.Value;

            return JsonConvert.DeserializeObject<List<int>>(permissions);
        }

        public long CurrentAccountId()
        {
            if (IsAuthenticated())
                return long.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "AccountId").Value);


            return 0;
        }

    }
}
