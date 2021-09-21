using API.DTOs.Distributors;
using API.DTOs.Roles;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
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
