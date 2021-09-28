using API.Domains;
using API.DTOs.Categories;
using API.DTOs.Distributors;
using API.DTOs.OrderDetails;
using API.DTOs.PaymentMethods;
using API.DTOs.Products;
using API.DTOs.RetailerPaymentMethods;
using API.DTOs.Retailers;
using API.DTOs.Roles;
using API.DTOs.Users;
using AutoMapper;

namespace API.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, CreateUserRequest>().ReverseMap();
            CreateMap<User, UpdateUserProfileRequest>().ReverseMap();
            CreateMap<User, GetUsersRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();

            CreateMap<Role, CreateRoleRequest>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();

            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<Category, CategoryHierachy>().ReverseMap();

            CreateMap<PaymentMethod, PaymentMethodResponse>().ReverseMap();
            CreateMap<PaymentMethod, CreatePaymentMethodRequest>().ReverseMap();

            CreateMap<RetailerPaymentMethod, RetailerPaymentMethodResponse>().ReverseMap();
            CreateMap<RetailerPaymentMethod, CreateRetailerPaymentMethodRequest>().ReverseMap();

            CreateMap<Distributor, DistributorResponse>().ReverseMap();

            CreateMap<Retailer, RetailerResponse>().ReverseMap();

            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Product, CreateProductRequest>().ReverseMap();
            CreateMap<Product, UpdateProductRequest>().ReverseMap();

            CreateMap<OrderDetail, CreateOrderDetailRequest>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();
        }
    }
}
