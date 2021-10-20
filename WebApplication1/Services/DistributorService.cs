using API.Contracts;
using API.Domains;
using API.DTOs.Distributors;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class DistributorService : IDistributorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DistributorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateDistributor(CreateDistributorRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                var userId = Guid.Parse(request.UserId);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                if (user != null)
                {
                    var role = await _unitOfWork.GetRepository<Role>().FirstAsync(x => x.Id.Equals(user.RoleId));
                    if (role.Name.Equals(Authorization.DT))
                    {
                        var distributor = await _unitOfWork.GetRepository<Distributor>().FirstAsync(x => x.UserId.Equals(userId));
                        if (distributor == null)
                        {
                            var newDistributor = new Distributor
                            {
                                DateCreated = DateTime.UtcNow,
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                UserId = userId
                            };
                            await _unitOfWork.GetRepository<Distributor>().AddAsync(newDistributor);
                            await _unitOfWork.SaveAsync();
                            return new Response<string>(newDistributor.Id.ToString(), message: "Distributor Created");
                        }
                    }
                }
            }
            return new Response<string>(message: "Failed to Create Distributor");
        }

        public async Task<Response<DistributorResponse>> GetDistributorById(GetDistributorByIdRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(Guid.Parse(request.Id));
                if (distributor != null && distributor.IsActive != false)
                {
                    return new Response<DistributorResponse>(_mapper.Map<DistributorResponse>(distributor), message: "Succeed");
                }
            }
            return new Response<DistributorResponse>("Failed");
        }

        public async Task<Response<IEnumerable<DistributorResponse>>> GetDistributors()
        {
            var distributor = await _unitOfWork.GetRepository<Distributor>().GetAllAsync();
            if (distributor.Any())
            {
                distributor = distributor.Where(x => x.IsActive == true);
                return new Response<IEnumerable<DistributorResponse>>(_mapper.Map<IEnumerable<DistributorResponse>>(distributor), message: "Success");
            }
            return new Response<IEnumerable<DistributorResponse>>(message: "Empty");
        }

        public async Task<Response<string>> UpdateDistributor(UpdateDistributorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                var distributor = await _unitOfWork.GetRepository<Distributor>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
                if (distributor != null)
                {
                    distributor.IsActive = request.IsActive;
                    distributor.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Distributor>().UpdateAsync(distributor);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(distributor.Id.ToString(), message: "Distributor Updated");
                }
            }
            return new Response<string>(message: "Fail to Update Distributor");
        }
    }
}
