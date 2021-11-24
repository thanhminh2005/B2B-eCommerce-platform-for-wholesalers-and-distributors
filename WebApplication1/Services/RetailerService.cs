using API.Contracts;
using API.Domains;
using API.DTOs.Retailers;
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
    public class RetailerService : IRetailerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RetailerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateRetailer(CreateRetailerRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(Guid.Parse(request.UserId));
                if (user != null)
                {
                    var role = await _unitOfWork.GetRepository<Role>().GetByIdAsync(user.RoleId);
                    if (role.Name.Equals(Authorization.RT))
                    {
                        var retailer = await _unitOfWork.GetRepository<Retailer>().GetByIdAsync(user.Id);
                        if (retailer == null)
                        {
                            var newRetailer = new Retailer
                            {
                                DateCreated = DateTime.UtcNow,
                                Id = Guid.NewGuid(),
                                IsActive = false,
                                UserId = user.Id
                            };
                            await _unitOfWork.GetRepository<Retailer>().AddAsync(newRetailer);
                            await _unitOfWork.SaveAsync();
                            return new Response<string>(newRetailer.Id.ToString(), message: "Retailer Created");
                        }
                    }
                }
            }
            return new Response<string>(message: "Failed to Create Retailer");
        }

        public async Task<Response<RetailerResponse>> GetRetailerById(GetRetailerByIdRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var distributor = await _unitOfWork.GetRepository<Retailer>().GetByIdAsync(Guid.Parse(request.Id));
                if (distributor != null)
                {
                    return new Response<RetailerResponse>(_mapper.Map<RetailerResponse>(distributor), message: "Succeed");
                }
            }
            return new Response<RetailerResponse>("Failed");
        }

        public async Task<Response<IEnumerable<RetailerResponse>>> GetRetailers()
        {
            var distributor = await _unitOfWork.GetRepository<Retailer>().GetAllAsync();
            if (distributor.Any())
            {
                distributor = distributor.Where(x => x.IsActive == true);
                return new Response<IEnumerable<RetailerResponse>>(_mapper.Map<IEnumerable<RetailerResponse>>(distributor), message: "Success");
            }
            return new Response<IEnumerable<RetailerResponse>>(message: "Empty");
        }

        public async Task<Response<string>> UpdateRetailer(UpdateRetailerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                var distributor = await _unitOfWork.GetRepository<Retailer>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
                if (distributor != null)
                {
                    distributor.IsActive = request.IsActive;
                    distributor.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Retailer>().UpdateAsync(distributor);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(distributor.Id.ToString(), message: "Retailer Updated");
                }
            }
            return new Response<string>(message: "Fail to Update Retailer");
        }
    }
}
