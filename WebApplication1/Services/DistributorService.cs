using API.Contracts;
using API.Domains;
using API.DTOs.Distributors;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
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
                                IsActive = false,
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
                if (distributor != null)
                {
                    return new Response<DistributorResponse>(_mapper.Map<DistributorResponse>(distributor), message: "Succeed");
                }
            }
            return new Response<DistributorResponse>("Failed");
        }

        public async Task<PagedResponse<IEnumerable<DistributorDisplayResponse>>> GetDistributors(GetDistributorsRequest request)
        {
            var distributors = await _unitOfWork.GetRepository<Distributor>().GetAsync(x =>
                (request.IsActive == null || x.IsActive == request.IsActive),
                orderBy: x => x.OrderBy(y => y.DateCreated),
                includeProperties: "User");
            var count = 0;
            IEnumerable<Distributor> response = null;
            if (string.IsNullOrWhiteSpace(request.Search))
            {
                if (request.HaveBusinessLicense == null)
                {
                    count = distributors.Count();
                    response = distributors.Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
                if (request.HaveBusinessLicense == false)
                {
                    count = distributors.Where(x => x.User.BusinessLicense == null).Count();
                    response = distributors.Where(x => x.User.BusinessLicense == null).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
                if (request.HaveBusinessLicense == true)
                {
                    count = distributors.Where(x => x.User.BusinessLicense != null).Count();
                    response = distributors.Where(x => x.User.BusinessLicense != null).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
            }
            else
            {
                if (request.HaveBusinessLicense == null)
                {
                    count = distributors.Where(x => x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search)).Count();
                    response = distributors.Where(x => x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search)).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
                if (request.HaveBusinessLicense == false)
                {
                    count = distributors.Where(x => (x.User.BusinessLicense == null) && (x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search))).Count();
                    response = distributors.Where(x => (x.User.BusinessLicense == null) && (x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search))).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }
                if (request.HaveBusinessLicense == true)
                {
                    count = distributors.Where(x => (x.User.BusinessLicense != null) && (x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search))).Count();
                    response = distributors.Where(x => (x.User.BusinessLicense != null) && (x.User.Email.Contains(request.Search) || x.User.DisplayName.Contains(request.Search))).Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize);
                }

            }
            return new PagedResponse<IEnumerable<DistributorDisplayResponse>>(_mapper.Map<IEnumerable<DistributorDisplayResponse>>(response), request.PageNumber, request.PageSize, count);
        }

        public async Task<Response<string>> UpdateDistributor(UpdateDistributorRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
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
