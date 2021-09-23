
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace B2B.Installers
{
    public class DependencyInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDistributorService, DistributorService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRetailerService, RetailerService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        }
    }
}