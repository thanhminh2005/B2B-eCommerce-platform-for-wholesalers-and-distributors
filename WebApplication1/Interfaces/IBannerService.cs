using API.DTOs.Banners;
using API.DTOs.Categories;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IBannerService
    {
        Task<Response<IEnumerable<BannerResponse>>> GetBanners();
        Task<Response<BannerResponse>> GetBannerById(GetBannerByIdRequest request);
        Task<Response<string>> CreateBanner(CreateBannerRequest request);
        Task<Response<string>> UpdateBanner(UpdateBannerRequest request);
        Task<Response<string>> DeleteBanner(DeleteBannerRequest request);
    }
}
