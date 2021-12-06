using API.Warppers;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IVNPayService
    {
        Task<Response<string>> SendIPNAsync(string vnpayData);
    }
}
