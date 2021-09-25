using API.DTOs.Retailers;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IRetailerService
    {
        Task<Response<string>> CreateRetailer(CreateRetailerRequest request);
        Task<Response<IEnumerable<RetailerResponse>>> GetRetailers();
        Task<Response<RetailerResponse>> GetRetailerById(GetRetailerByIdRequest request);
        Task<Response<string>> UpdateRetailer(UpdateRetailerRequest request);
    }
}
