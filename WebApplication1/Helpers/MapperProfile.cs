using API.Domains;
using API.DTOs.Categories;
using API.DTOs.Distributors;
using API.DTOs.Roles;
using API.DTOs.Users;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            CreateMap<Distributor, DistributorResponse>().ReverseMap();

        }
    }
}
