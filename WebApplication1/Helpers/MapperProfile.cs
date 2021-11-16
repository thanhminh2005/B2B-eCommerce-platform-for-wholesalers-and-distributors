using API.Domains;
using API.DTOs.Banners;
using API.DTOs.Categories;
using API.DTOs.CustomerRanks;
using API.DTOs.Distributors;
using API.DTOs.HomeBanners;
using API.DTOs.MembershipRanks;
using API.DTOs.Memberships;
using API.DTOs.OrderDetails;
using API.DTOs.Orders;
using API.DTOs.PaymentMethods;
using API.DTOs.Prices;
using API.DTOs.Products;
using API.DTOs.Retailers;
using API.DTOs.Roles;
using API.DTOs.Sessions;
using API.DTOs.SubCategories;
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
            CreateMap<User, UserDisplayResponse>().ReverseMap();

            CreateMap<Role, CreateRoleRequest>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();

            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CreateCategoryRequest>().ReverseMap();

            CreateMap<SubCategory, SubCategoryResponse>().ReverseMap();
            CreateMap<SubCategory, CreateSubCategoryRequest>().ReverseMap();

            CreateMap<PaymentMethod, PaymentMethodResponse>().ReverseMap();
            CreateMap<PaymentMethod, CreatePaymentMethodRequest>().ReverseMap();


            CreateMap<Distributor, DistributorResponse>().ReverseMap();
            CreateMap<Distributor, DistributorDisplayResponse>().ReverseMap();

            CreateMap<Retailer, RetailerResponse>().ReverseMap();
            CreateMap<Retailer, RetailerDisplayResponse>().ReverseMap();

            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Product, CreateProductRequest>().ReverseMap();
            CreateMap<Product, UpdateProductRequest>().ReverseMap();
            CreateMap<Product, RetailerGetProductsResponse>().ReverseMap();

            CreateMap<OrderDetail, CreateOrderDetailRequest>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();

            CreateMap<Order, CreateOrderRequest>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();

            CreateMap<Session, CreateSessionRequest>().ReverseMap();
            CreateMap<Session, SessionResponse>().ReverseMap();

            CreateMap<Price, PriceResponse>().ReverseMap();
            CreateMap<Price, CreatePriceRequest>().ReverseMap();

            CreateMap<Banner, BannerResponse>().ReverseMap();
            CreateMap<Banner, CreateBannerRequest>().ReverseMap();
            CreateMap<Banner, UpdateBannerRequest>().ReverseMap();

            CreateMap<HomeBanner, HomeBannerResponse>().ReverseMap();
            CreateMap<HomeBanner, CreateHomeBannerRequest>().ReverseMap();
            CreateMap<HomeBanner, UpdateHomeBannerRequest>().ReverseMap();

            CreateMap<MembershipRank, MembershipRankResponse>().ReverseMap();
            CreateMap<MembershipRank, CreateMembershipRankRequest>().ReverseMap();

            CreateMap<CustomerRank, CustomerRankResponse>().ReverseMap();
            CreateMap<CustomerRank, CreateCustomerRankRequest>().ReverseMap();

            CreateMap<Membership, MembershipResponse>().ReverseMap();
            CreateMap<Membership, DistributorMembershipResponse>().ReverseMap();
            CreateMap<Membership, RetailerMembershipResponse>().ReverseMap();
            CreateMap<Membership, CreateMembershipRequest>().ReverseMap();
        }
    }
}
