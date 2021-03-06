using API.Domains;
using API.DTOs.CustomerRanks;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class CustomerRankService : ICustomerRankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerRankService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateCustomerRank(CreateCustomerRankRequest request)
        {
            var customerRank = await _unitOfWork.GetRepository<CustomerRank>().FirstAsync(c => c.DistributorId.Equals(request.DistributorId)
                                                                                               && c.MembershipRankId.Equals(request.MembershipRankId));
            if (customerRank == null && request.Threshold > 0 && request.DiscountRate > 0)
            {
                var ranks = await _unitOfWork.GetRepository<CustomerRank>().GetAsync(x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)));
                if (ranks != null)
                {
                    if (!ranks.Any(x => x.Threshold == request.Threshold))
                    {
                        var newCustomerRank = _mapper.Map<CustomerRank>(request);
                        newCustomerRank.Id = Guid.NewGuid();
                        newCustomerRank.Threshold = request.Threshold;
                        newCustomerRank.DiscountRate = request.DiscountRate;
                        newCustomerRank.DateCreated = DateTime.UtcNow;
                        await _unitOfWork.GetRepository<CustomerRank>().AddAsync(newCustomerRank);
                        await _unitOfWork.SaveAsync();
                        var memberships = await _unitOfWork.GetRepository<Membership>().GetAsync(x => x.DistributorId.Equals(customerRank.DistributorId));
                        var customerRanks = await _unitOfWork.GetRepository<CustomerRank>().GetAsync(x => x.DistributorId.Equals(customerRank.DistributorId), orderBy: x => x.OrderBy(y => y.Threshold));
                        if (memberships.Any())
                        {
                            foreach (var membership in memberships)
                            {
                                if (customerRanks.Any())
                                {
                                    customerRanks.OrderBy(x => x.Threshold);
                                    foreach (var rank in customerRanks)
                                    {
                                        if (rank.Threshold <= membership.Point)
                                        {
                                            membership.MembershipRankId = rank.MembershipRankId;
                                        }

                                    }
                                    _unitOfWork.GetRepository<Membership>().UpdateAsync(membership);
                                }
                            }
                            await _unitOfWork.SaveAsync();
                        }
                        return new Response<string>(newCustomerRank.Id.ToString(), message: "Customer's Rank Created");
                    }
                }
            }
            return new Response<string>(message: "Failed to Create");
        }

        public async Task<Response<CustomerRankResponse>> GetCustomerRankById(GetCustomerRankByIdRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var customerRank = await _unitOfWork.GetRepository<CustomerRank>().GetByIdAsync(Guid.Parse(request.Id));
                if (customerRank != null)
                {
                    return new Response<CustomerRankResponse>(_mapper.Map<CustomerRankResponse>(customerRank), message: "Success");
                }
            }
            return new Response<CustomerRankResponse>(message: "CustomerRank not Found");
        }

        public async Task<Response<IEnumerable<CustomerRankResponse>>> GetCustomerRanks(GetCustomerRanksRequest request)
        {
            var customerRanks = await _unitOfWork.GetRepository<CustomerRank>().GetAsync(filter: x => request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)));
            if (customerRanks.Any())
            {
                return new Response<IEnumerable<CustomerRankResponse>>(_mapper.Map<IEnumerable<CustomerRankResponse>>(customerRanks), message: "Success");
            }
            return new Response<IEnumerable<CustomerRankResponse>>(message: "Empty");
        }

        public async Task<Response<string>> UpdateCustomerRank(UpdateCustomerRankRequest request)
        {
            var customerRank = await _unitOfWork.GetRepository<CustomerRank>().GetByIdAsync(Guid.Parse(request.Id));
            if (customerRank != null && request.Threshold > 0 && request.DiscountRate > 0)
            {
                var ranks = await _unitOfWork.GetRepository<CustomerRank>().GetAsync(x => x.DistributorId.Equals(customerRank.DistributorId));
                if (ranks != null)
                {
                    if (!ranks.Any(x => x.Threshold == request.Threshold) || !ranks.Any(x => x.DiscountRate == request.DiscountRate))
                    {
                        customerRank.DateModified = DateTime.UtcNow;
                        customerRank.MembershipRankId = Guid.Parse(request.MembershipRankId);
                        customerRank.Threshold = request.Threshold;
                        customerRank.DiscountRate = request.DiscountRate;
                        _unitOfWork.GetRepository<CustomerRank>().UpdateAsync(customerRank);
                        await _unitOfWork.SaveAsync();
                        var memberships = await _unitOfWork.GetRepository<Membership>().GetAsync(x => x.DistributorId.Equals(customerRank.DistributorId));
                        var customerRanks = await _unitOfWork.GetRepository<CustomerRank>().GetAsync(x => x.DistributorId.Equals(customerRank.DistributorId), orderBy: x => x.OrderBy(y => y.Threshold));
                        if (memberships.Any())
                        {
                            foreach (var membership in memberships)
                            {
                                if (customerRanks.Any())
                                {
                                    foreach (var rank in customerRanks)
                                    {
                                        if (rank.Threshold <= membership.Point)
                                        {
                                            membership.MembershipRankId = rank.MembershipRankId;
                                        }

                                    }
                                    _unitOfWork.GetRepository<Membership>().UpdateAsync(membership);
                                }
                            }
                            await _unitOfWork.SaveAsync();
                        }
                        return new Response<string>(request.Id, message: "Customer's Rank Updated");
                    }
                }
            }
            return new Response<string>(message: "Customer's Rank not Found");
        }
    }
}
