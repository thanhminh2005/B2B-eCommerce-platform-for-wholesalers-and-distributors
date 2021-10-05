using API.DTOs.Banners;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class BannerService : IBannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Task<Response<string>> CreateBanner(CreateBannerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> DeleteBanner(DeleteBannerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BannerResponse>> GetBannerById(GetBannerByIdRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<BannerResponse>>> GetBanners()
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> UpdateBanner(UpdateBannerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
