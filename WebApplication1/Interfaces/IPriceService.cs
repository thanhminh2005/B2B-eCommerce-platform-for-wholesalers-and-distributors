using API.DTOs.Prices;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IPriceService
    {
        Task<Response<string>> CreatePrice(CreatePriceRequest request);
        Task<Response<IEnumerable<PriceResponse>>> GetPrices(GetPricesRequest request);
        Task<Response<PriceResponse>> GetPriceById(GetPriceByIdRequest request);
        Task<Response<string>> UpdatePrice(UpdatePriceRequest request);
        Task<Response<string>> DeletePrice(DeletePriceRequest request);
    }
}
