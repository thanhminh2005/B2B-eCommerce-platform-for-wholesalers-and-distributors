using API.Domains;
using API.DTOs.MembershipRanks;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class MembershipRankService : IMembershipRankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipRankService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateMembershipRank(CreateMembershipRankRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.RankName))
            {
                var membershipRank = await _unitOfWork.GetRepository<MembershipRank>().FirstAsync(c => c.RankName.Equals(request.RankName));
                if (membershipRank == null)
                {
                    var newMembershipRank = _mapper.Map<MembershipRank>(request);
                    newMembershipRank.Id = Guid.NewGuid();
                    newMembershipRank.DateCreated = DateTime.UtcNow;
                    await _unitOfWork.GetRepository<MembershipRank>().AddAsync(newMembershipRank);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(request.RankName, message: "Membership's Rank Created");
                }
            }
            return new Response<string>(message: "Failed to Create");
        }

        public async Task<Response<MembershipRankResponse>> GetMembershipRankById(GetMembershipRankByIdRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var membershipRank = await _unitOfWork.GetRepository<MembershipRank>().GetByIdAsync(Guid.Parse(request.Id));
                if (membershipRank != null)
                {
                    return new Response<MembershipRankResponse>(_mapper.Map<MembershipRankResponse>(membershipRank), message: "Success");
                }
            }
            return new Response<MembershipRankResponse>(message: "MembershipRank not Found");
        }

        public async Task<Response<IEnumerable<MembershipRankResponse>>> GetMembershipRanks()
        {
            var membershipRanks = await _unitOfWork.GetRepository<MembershipRank>().GetAllAsync();
            if (membershipRanks != null)
            {
                return new Response<IEnumerable<MembershipRankResponse>>(_mapper.Map<IEnumerable<MembershipRankResponse>>(membershipRanks), message: "Success");
            }
            return new Response<IEnumerable<MembershipRankResponse>>(message: "Empty");
        }

        public async Task<Response<string>> UpdateMembershipRank(UpdateMembershipRankRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id) && !string.IsNullOrWhiteSpace(request.RankName))
            {
                var membershipRank = await _unitOfWork.GetRepository<MembershipRank>().GetByIdAsync(Guid.Parse(request.Id));
                if (membershipRank != null)
                {
                    membershipRank.DateModified = DateTime.UtcNow;
                    membershipRank.RankName = request.RankName;
                    _unitOfWork.GetRepository<MembershipRank>().UpdateAsync(membershipRank);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(membershipRank.RankName, message: "Membership's Rank Updated");
                }
            }
            return new Response<string>(message: "Membership's Rank not Found");
        }
    }
}
