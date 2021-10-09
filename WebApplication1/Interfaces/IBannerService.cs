using API.DTOs.Banners;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IBannerService
    {
        Task<Response<IEnumerable<BannerResponse>>> GetBannersByDistributor(GetBannerByDistributorRequest request);
        Task<Response<BannerResponse>> GetBannerById(GetBannerByIdRequest request);
        Task<Response<string>> CreateBanner(CreateBannerRequest request);
        Task<Response<string>> UpdateBanner(UpdateBannerRequest request);
        Task<Response<string>> DeleteBanner(DeleteBannerRequest request);
        Task<Response<IEnumerable<BannerResponse>>> GetBanners();
    }
}
