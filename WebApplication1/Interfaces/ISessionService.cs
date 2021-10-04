using API.DTOs.Sessions;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ISessionService
    {
        Task<Response<string>> CreateSession(CreateSessionRequest request);
        Task<Response<IEnumerable<SessionResponse>>> GetSessions(GetSessionsRequest request);
        Task<Response<SessionResponse>> GetSessionById(GetSessionByIdRequest request);
        Task<Response<string>> UpdateSession(UpdateSessionRequest request);
        Task<Response<string>> DeleteSession(DeleteSessionRequest request);
    }
}
