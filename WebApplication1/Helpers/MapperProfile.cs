using API.Domains;
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
            CreateMap<User, CreateUserRequest>();
            CreateMap<User, UpdateUserRequest>();
            CreateMap<User, GetUsersRequest>();
            CreateMap<User, UserResponse>();

        }
    }
}
