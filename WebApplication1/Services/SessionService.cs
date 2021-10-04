using API.DTOs.Sessions;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<Response<string>> CreateSession(CreateSessionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> DeleteSession(DeleteSessionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SessionResponse>> GetSessionById(GetSessionByIdRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<SessionResponse>>> GetSessions(GetSessionsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> UpdateSession(UpdateSessionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
