using API.MoMo;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMoMoPaymentService
    {
        Task<IPNResponse> GetPaymentStatusAsync(IPNRequest request);

    }
}
