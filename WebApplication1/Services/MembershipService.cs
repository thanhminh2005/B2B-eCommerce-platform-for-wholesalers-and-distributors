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
                var customerRank = await _unitOfWork.GetRepository<CustomerRank>().FirstAsync(x => x.DistributorId.Equals(membership.DistributorId) && x.MembershipRankId.Equals(membership.MembershipRankId));
                if (customerRank != null)
                {
                    var response = _mapper.Map<MembershipResponse>(membership);
                    response.DiscountRate = customerRank.DiscountRate;
                    return new Response<MembershipResponse>(response, message: "Success");
                }
            }
            return new Response<MembershipResponse>(message: "Not found");
        }

        public async Task<PagedResponse<IEnumerable<object>>> GetMemberships(GetMembershipsRequest request)
        {
            IEnumerable<Membership> memberships = null;
            var count = 0;
            if (!string.IsNullOrWhiteSpace(request.DistributorId) && string.IsNullOrWhiteSpace(request.RetailerId))
            {
                memberships = await _unitOfWork.GetRepository<Membership>().GetPagedReponseAsync(request.PageNumber,
                                                                                                 request.PageSize,
                                                                                                 filter: x => (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                                              && (request.MembershipRankId == null || x.MembershipRankId.Equals(Guid.Parse(request.MembershipRankId))),
                                                                                                 orderBy: x => x.OrderByDescending(y => y.Point),
                                                                                                 includeProperties: "Retailer,MembershipRank");
                count = await _unitOfWork.GetRepository<Membership>().CountAsync(x => (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                    && (request.MembershipRankId == null || x.MembershipRankId.Equals(Guid.Parse(request.MembershipRankId))));
                foreach (var member in memberships)
                {
                    var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(member.Retailer.UserId);
                    member.Retailer.User = user;
                }
                return new PagedResponse<IEnumerable<object>>(_mapper.Map<IEnumerable<DistributorMembershipResponse>>(memberships), request.PageNumber, request.PageSize, count);
            }
            if (string.IsNullOrWhiteSpace(request.DistributorId) && !string.IsNullOrWhiteSpace(request.RetailerId))
            {
                memberships = await _unitOfWork.GetRepository<Membership>().GetPagedReponseAsync(request.PageNumber,
                                                                                                 request.PageSize,
                                                                                                 filter: x => (request.RetailerId == null || x.RetailerId.Equals(Guid.Parse(request.RetailerId)))
                                                                                                              && (request.MembershipRankId == null || x.MembershipRankId.Equals(Guid.Parse(request.MembershipRankId))),
                                                                                                 orderBy: x => x.OrderByDescending(y => y.Point),
                                                                                                 includeProperties: "Distributor,MembershipRank");
                count = await _unitOfWork.GetRepository<Membership>().CountAsync(x => (request.RetailerId == null || x.RetailerId.Equals(Guid.Parse(request.RetailerId)))
                                                                                    && (request.MembershipRankId == null || x.MembershipRankId.Equals(Guid.Parse(request.MembershipRankId))));
                foreach (var member in memberships)
                {
                    var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(member.Distributor.UserId);
                    member.Distributor.User = user;
                }
                return new PagedResponse<IEnumerable<object>>(_mapper.Map<IEnumerable<DistributorMembershipResponse>>(memberships), request.PageNumber, request.PageSize, count);
            }

            return new PagedResponse<IEnumerable<object>>(memberships, request.PageNumber, request.PageSize, count);
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
