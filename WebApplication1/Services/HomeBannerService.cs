using API.Domains;
using API.DTOs.HomeBanners;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class HomeBannerService : IHomeBannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeBannerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateHomeBanner(CreateHomeBannerRequest request)
        {
            if (request != null)
            {
                var homeBanners = await _unitOfWork.GetRepository<HomeBanner>().FirstAsync(x => x.Name.Equals(request.Name));
                if (homeBanners == null)
                {
                    if (request.Position < 0 || request.Position == 0 || request.Position > 5)
                    {
                        return new Response<string>(message: "HomeBanner position must be from range 1 to 5");
                    }
                    else
                    {
                        HomeBanner newHomeBanner = _mapper.Map<HomeBanner>(request);
                        newHomeBanner.Id = Guid.NewGuid();
                        var ExistedHomeBanners = await _unitOfWork.GetRepository<HomeBanner>().GetAsync(
                                                                             orderBy: x => x.OrderBy(y => y.Position));
                        var count = await _unitOfWork.GetRepository<HomeBanner>().CountAsync();
                        if (count == 0)
                        {
                            newHomeBanner.Position = 1;
                        }
                        else
                        {
                            //update homeBanners's position to the lowest empty position
                            foreach (HomeBanner homeBanners1 in ExistedHomeBanners)
                            {
                                if (request.Position == homeBanners1.Position)
                                {
                                    request.Position = homeBanners1.Position + 1;
                                }
                            }
                            newHomeBanner.Position = request.Position;
                            if (newHomeBanner.Position > 5)
                            {
                                return new Response<string>(message: "You have reached maximum size of homeBanners, please remove another homeBanners to add the new one");
                            }
                        }
                        newHomeBanner.DateCreated = DateTime.UtcNow;
                        await _unitOfWork.GetRepository<HomeBanner>().AddAsync(newHomeBanner);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(newHomeBanner.Name, message: "HomeBanner created successfully ");
                    }
                }
                return new Response<string>(message: "HomeBanner name is existed");
            }
            return new Response<string>(message: "Failed to create homeBanners");
        }

        public async Task<Response<string>> DeleteHomeBanner(DeleteHomeBannerRequest request)
        {
            var homeBanner = await _unitOfWork.GetRepository<HomeBanner>().GetByIdAsync(Guid.Parse(request.Id));
            if (homeBanner != null)
            {
                var homeBanners = await _unitOfWork.GetRepository<HomeBanner>().GetAsync(orderBy: x => x.OrderBy(y => y.Position));
                if (homeBanners.Count() > 1)
                {
                    if (homeBanner.Position < homeBanners.Count())
                    {
                        foreach (var current in homeBanners)
                        {
                            if (current.Position > homeBanner.Position)
                            {
                                current.Position -= 1;
                                _unitOfWork.GetRepository<HomeBanner>().UpdateAsync(current);
                                await _unitOfWork.SaveAsync();
                            }
                        }
                    }
                }
                _unitOfWork.GetRepository<HomeBanner>().DeleteAsync(homeBanner);
                await _unitOfWork.SaveAsync();
                return new Response<string>(homeBanner.Id.ToString(), homeBanner.Name + " is deleted successfully");
            }
            return new Response<string>("HomeBanner deleted failed");
        }

        public async Task<Response<HomeBannerResponse>> GetHomeBannerById(GetHomeBannerByIdRequest request)
        {
            var homeBanners = await _unitOfWork.GetRepository<HomeBanner>().GetByIdAsync(Guid.Parse(request.Id));

            if (homeBanners != null)
            {
                return new Response<HomeBannerResponse>(_mapper.Map<HomeBannerResponse>(homeBanners), message: "Success");
            }
            return new Response<HomeBannerResponse>("HomeBanner Not Found");
        }

        public async Task<Response<IEnumerable<HomeBannerResponse>>> GetHomeBanners()
        {
            var homeBannerss = await _unitOfWork.GetRepository<HomeBanner>().GetAllAsync();
            var count = await _unitOfWork.GetRepository<HomeBanner>().CountAsync();
            if (count != 0)
            {
                return new Response<IEnumerable<HomeBannerResponse>>(_mapper.Map<IEnumerable<HomeBannerResponse>>(homeBannerss), message: "Success");
            }
            return new Response<IEnumerable<HomeBannerResponse>>(_mapper.Map<IEnumerable<HomeBannerResponse>>(homeBannerss), message: "Empty");
        }

        public async Task<Response<string>> UpdateHomeBanner(UpdateHomeBannerRequest request)
        {
            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Id))
                {
                    HomeBanner newHomeBanner = await _unitOfWork.GetRepository<HomeBanner>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
                    if (request.Position < 0 || request.Position == 0 || request.Position > 5)
                    {
                        return new Response<string>(message: "HomeBanner position must be from range 1 to 5");
                    }
                    else
                    {
                        newHomeBanner.Name = request.Name;
                        newHomeBanner.Link = request.Link;
                        newHomeBanner.Image = request.Image;
                        var ExistedHomeBanners = await _unitOfWork.GetRepository<HomeBanner>().GetAsync(orderBy: x => x.OrderBy(y => y.Position));
                        foreach (HomeBanner homeBanners1 in ExistedHomeBanners)
                        {
                            if (request.Position == homeBanners1.Position && newHomeBanner.Id != homeBanners1.Id)
                            {
                                request.Position = homeBanners1.Position + 1;
                            }
                        }
                        newHomeBanner.Position = request.Position;
                        newHomeBanner.DateModified = DateTime.UtcNow;
                        _unitOfWork.GetRepository<HomeBanner>().UpdateAsync(newHomeBanner);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(newHomeBanner.Id.ToString(), message: "HomeBanner is updated");
                    }
                }
                return new Response<string>(message: "HomeBanner ID can not be blanked");
            }
            return new Response<string>(message: "Fail to update homeBanners");
        }
    }
}
