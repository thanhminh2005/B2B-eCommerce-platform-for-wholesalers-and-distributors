using API.DTOs.HomeBanners;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IHomeBannerService
    {
        Task<Response<HomeBannerResponse>> GetHomeBannerById(GetHomeBannerByIdRequest request);
        Task<Response<string>> CreateHomeBanner(CreateHomeBannerRequest request);
        Task<Response<string>> UpdateHomeBanner(UpdateHomeBannerRequest request);
        Task<Response<string>> DeleteHomeBanner(DeleteHomeBannerRequest request);
        Task<Response<IEnumerable<HomeBannerResponse>>> GetHomeBanners();
    }
}
