using API.DTOs.CustomerRanks;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ICustomerRankService
    {
        Task<Response<IEnumerable<CustomerRankResponse>>> GetCustomerRanks(GetCustomerRanksRequest request);
        Task<Response<CustomerRankResponse>> GetCustomerRankById(GetCustomerRankByIdRequest request);
        Task<Response<string>> CreateCustomerRank(CreateCustomerRankRequest request);
        Task<Response<string>> UpdateCustomerRank(UpdateCustomerRankRequest request);
    }
}
