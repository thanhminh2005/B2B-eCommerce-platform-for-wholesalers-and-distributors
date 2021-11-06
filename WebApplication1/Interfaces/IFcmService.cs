using API.DTOs.Fcms;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IFcmService
    {
        Task<Response<string>> CreateFcm(CreateFcmRequest request);
        Task<Response<IEnumerable<FcmResponse>>> GetFcms(GetFcmsRequest request);
        Task<Response<FcmResponse>> GetFcmById(GetFcmByIdRequest request);
        Task<Response<string>> UpdateFcm(UpdateFcmRequest request);
        Task<Response<string>> DeleteFcm(DeleteFcmRequest request);
    }
}
