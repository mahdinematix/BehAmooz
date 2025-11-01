using _01_Framework.Infrastructure;
using AccountManagement.Application;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Application.Contract.Wallet;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Domain.WalletAgg;
using AccountManagement.Infrastructure.Configuration.Permission;
using AccountManagement.Infrastructure.EFCore;
using AccountManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Infrastructure.Configuration
{
    public class AccountManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IAccountApplication, AccountApplication>();
            services.AddTransient<IAccountRepository, AccountRepository>();

            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRoleApplication, RoleApplication>();
            services.AddTransient<IPermissionExposer, AccountPermissionExposer>();

            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddTransient<IWalletApplication, WalletApplication>();


            services.AddDbContext<AccountContext>(x => x.UseSqlServer(connectionString));
        }

    }
}
