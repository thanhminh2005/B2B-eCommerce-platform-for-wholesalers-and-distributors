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
                var retailer = await _unitOfWork.GetRepository<Retailer>().GetByIdAsync(Guid.Parse(request.Id));
                if (retailer != null)
                {
                    return new Response<RetailerResponse>(_mapper.Map<RetailerResponse>(retailer), message: "Succeed");
                }
            }
            return new Response<RetailerResponse>("Failed");
        }

        public async Task<PagedResponse<IEnumerable<RetailerDisplayResponse>>> GetRetailers(GetRetailersRequest request)
        {
            var retailers = await _unitOfWork.GetRepository<Retailer>().GetAsync(x =>
                (request.IsActive == null || x.IsActive == request.IsActive),
                orderBy: x => x.OrderBy(y => y.DateCreated),
                includeProperties: "User");
            var count = 0;
            IEnumerable<Retailer> response = null;
            if (string.IsNullOrWhiteSpace(request.Search))
            {
                if (request.HaveBusinessLicense == null)
                {
                    count = retailers.Count();
                    response = retailers.Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
                if (request.HaveBusinessLicense == false)
                {
                    count = retailers.Where(x => x.User.BusinessLicense == null).Count();
                    response = retailers.Where(x => x.User.BusinessLicense == null).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
                if (request.HaveBusinessLicense == true)
                {
                    count = retailers.Where(x => x.User.BusinessLicense != null).Count();
                    response = retailers.Where(x => x.User.BusinessLicense != null).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
            }
            else
            {
                if (request.HaveBusinessLicense == null)
                {
                    count = retailers.Where(x => x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search)).Count();
                    response = retailers.Where(x => x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search)).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
                if (request.HaveBusinessLicense == false)
                {
                    count = retailers.Where(x => (x.User.BusinessLicense == null) && (x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search))).Count();
                    response = retailers.Where(x => (x.User.BusinessLicense == null) && (x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search))).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
                if (request.HaveBusinessLicense == true)
                {
                    count = retailers.Where(x => (x.User.BusinessLicense != null) && (x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search))).Count();
                    response = retailers.Where(x => (x.User.BusinessLicense != null) && (x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search))).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }

            }
            return new PagedResponse<IEnumerable<RetailerDisplayResponse>>(_mapper.Map<IEnumerable<RetailerDisplayResponse>>(response), request.PageNumber, request.PageSize, count);
        }
        public async Task<Response<string>> UpdateRetailer(UpdateRetailerRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var retailer = await _unitOfWork.GetRepository<Retailer>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
                if (retailer != null)
                {
                    retailer.IsActive = request.IsActive;
                    retailer.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Retailer>().UpdateAsync(retailer);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(retailer.Id.ToString(), message: "Retailer Updated");
                }
            }
            return new Response<string>(message: "Fail to Update Retailer");
        }
    }
}
