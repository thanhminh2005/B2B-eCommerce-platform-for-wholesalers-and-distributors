using API.MoMo;
using API.Warppers;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMoMoPaymentService
    {
        Task<Response<string>> GetPaymentStatusAsync(IPNRequest request);

    }
}
