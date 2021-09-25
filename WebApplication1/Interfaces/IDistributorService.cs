using API.DTOs.Distributors;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IDistributorService
    {
        Task<Response<string>> CreateDistributor(CreateDistributorRequest request);
        Task<Response<IEnumerable<DistributorResponse>>> GetDistributors();
        Task<Response<DistributorResponse>> GetDistributorById(GetDistributorByIdRequest request);
        Task<Response<string>> UpdateDistributor(UpdateDistributorRequest request);
    }
}
