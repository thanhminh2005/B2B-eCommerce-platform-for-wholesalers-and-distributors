using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Installers
{
    public class DependencyInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDistributorService, DistributorService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRetailerService, RetailerService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<IMembershipRankService, MembershipRankService>();
            services.AddScoped<ICustomerRankService, CustomerRankService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IMoMoPaymentService, MoMoPaymentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IHomeBannerService, HomeBannerService>();
            services.AddScoped<IVNPayService, VNPayService>();
        }
    }
}