using API.Domains;
using API.DTOs.Memberships;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateMembership(CreateMembershipRequest request)
        {
            var membership = await _unitOfWork.GetRepository<Membership>().FirstAsync(c => c.DistributorId.Equals(request.DistributorId)
                                                                                               && c.MembershipRankId.Equals(request.MembershipRankId));
            if (membership == null)
            {
                var newMembership = _mapper.Map<Membership>(request);
                newMembership.Id = Guid.NewGuid();
                newMembership.DateCreated = DateTime.UtcNow;
                await _unitOfWork.GetRepository<Membership>().AddAsync(newMembership);
                await _unitOfWork.SaveAsync();
                return new Response<string>(newMembership.Id.ToString(), message: "Membership Created");
            }
            return new Response<string>(message: "Failed to Create");
        }

        public async Task<Response<MembershipResponse>> GetMembershipById(GetMembershipByIdRequest request)
        {
                var membership = await _unitOfWork.GetRepository<Membership>().FirstAsync(x => x.DistributorId.Equals(Guid.Parse(request.DistributorId))
                                                                                               && x.RetailerId.Equals(Guid.Parse(request.RetailerId)));
                if (membership != null)
                {
                    return new Response<MembershipResponse>(_mapper.Map<MembershipResponse>(membership), message: "Success");
                }
            return new Response<MembershipResponse>(message: "Not found");
        }

        public async Task<PagedResponse<IEnumerable<MembershipResponse>>> GetMemberships(GetMembershipsRequest request)
        {
            var memberships = await _unitOfWork.GetRepository<Membership>().GetPagedReponseAsync(request.PageNumber,
                                                                                                 request.PageSize,
                                                                                                 filter: x => (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                                              && (request.RetailerId == null || x.Retailer.Equals(Guid.Parse(request.RetailerId)))
                                                                                                              && (request.MembershipRankId == null || x.Retailer.Equals(Guid.Parse(request.MembershipRankId))),
                                                                                                 orderBy: x => x.OrderByDescending(y => y.Point));
            var count = await _unitOfWork.GetRepository<Membership>().CountAsync(x => (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                                              && (request.RetailerId == null || x.Retailer.Equals(Guid.Parse(request.RetailerId)))
                                                                                                              && (request.MembershipRankId == null || x.Retailer.Equals(Guid.Parse(request.MembershipRankId))));
            return new PagedResponse<IEnumerable<MembershipResponse>>(_mapper.Map<IEnumerable<MembershipResponse>>(memberships), request.PageNumber, request.PageSize, count);
        }

        public async Task<Response<string>> UpdateMembership(UpdateMembershipRequest request)
        {
            var membership = await _unitOfWork.GetRepository<Membership>().FirstAsync(c => c.DistributorId.Equals(request.DistributorId)
                                                                                               && c.MembershipRankId.Equals(request.MembershipRankId));
            if (membership != null)
            {
                membership.Point = request.Point;
                membership.MembershipRankId = Guid.Parse(request.MembershipRankId);
                membership.DateModified = DateTime.UtcNow;
                _unitOfWork.GetRepository<Membership>().UpdateAsync(membership);
                await _unitOfWork.SaveAsync();
                return new Response<string>(membership.Id.ToString(), message: "Membership Updated");
            }
            return new Response<string>(message: "Membership not Found");
        }
    }
}
