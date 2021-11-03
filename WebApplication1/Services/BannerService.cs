using API.Domains;
using API.DTOs.Banners;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class BannerService : IBannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BannerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateBanner(CreateBannerRequest request)
        {
            if (request != null)
            {
                var banner = await _unitOfWork.GetRepository<Banner>().FirstAsync(x => x.Name.Equals(request.Name));
                if (banner == null)
                {
                    var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(Guid.Parse(request.DistributorId));
                    if (distributor == null)
                    {
                        return new Response<string>(message: "Distributor is not existed ");
                    }
                    else if (distributor.IsActive)
                    {
                        if (request.Position < 0 || request.Position == 0 || request.Position > 5)
                        {
                            return new Response<string>(message: "Banner position must be from range 1 to 5");
                        }
                        else
                        {
                            Banner newBanner = _mapper.Map<Banner>(request);
                            newBanner.Id = Guid.NewGuid();
                            newBanner.DistributorId = Guid.Parse(request.DistributorId);
                            var ExistedBanners = await _unitOfWork.GetRepository<Banner>().GetAsync(filter: x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)),
                                                                                 orderBy: x => x.OrderBy(y => y.Position));
                            var count = await _unitOfWork.GetRepository<Banner>().CountAsync(filter: x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)));
                            if (count == 0)
                            {
                                newBanner.Position = 1;
                            }
                            else
                            {
                                //update banner's position to the lowest empty position
                                foreach (Banner banner1 in ExistedBanners)
                                {
                                    if (request.Position == banner1.Position)
                                    {
                                        request.Position = banner1.Position + 1;
                                    }
                                }
                                newBanner.Position = request.Position;
                                if (newBanner.Position > 5)
                                {
                                    return new Response<string>(message: "You have reached maximum size of banner, please remove another banner to add the new one");
                                }
                            }
                            newBanner.DateCreated = DateTime.UtcNow;
                            await _unitOfWork.GetRepository<Banner>().AddAsync(newBanner);
                            await _unitOfWork.SaveAsync();
                            return new Response<string>(newBanner.Name, message: "Banner created successfully ");
                        }
                    }
                    return new Response<string>(message: "Distributor is removed ");
                }
                return new Response<string>(message: "Banner name is existed");
            }
            return new Response<string>(message: "Failed to create banner");
        }

        public async Task<Response<string>> DeleteBanner(DeleteBannerRequest request)
        {
            var banner = await _unitOfWork.GetRepository<Banner>().GetByIdAsync(Guid.Parse(request.Id));
            if (banner != null)
            {
                var banners = await _unitOfWork.GetRepository<Banner>().GetAsync(x => x.DistributorId.Equals(banner.DistributorId), orderBy: x => x.OrderBy(y => y.Position));
                if(banners.Count() > 1)
                {
                    if(banner.Position < banners.Count())
                    {
                        foreach(var current in banners)
                        {
                            if(current.Position > banner.Position)
                            {
                                current.Position -= 1;
                                _unitOfWork.GetRepository<Banner>().UpdateAsync(current);
                                await _unitOfWork.SaveAsync();
                            }
                        }
                    }
                }
                _unitOfWork.GetRepository<Banner>().DeleteAsync(banner);
                await _unitOfWork.SaveAsync();
                return new Response<string>(banner.DistributorId.ToString(), banner.Name + " is deleted successfully");
            }
            return new Response<string>("Banner deleted failed");
        }

        public async Task<Response<BannerResponse>> GetBannerById(GetBannerByIdRequest request)
        {
            var banner = await _unitOfWork.GetRepository<Banner>().GetByIdAsync(Guid.Parse(request.Id));

            if (banner != null)
            {
                return new Response<BannerResponse>(_mapper.Map<BannerResponse>(banner), message: "Success");
            }
            return new Response<BannerResponse>("Banner Not Found");
        }

        public async Task<Response<IEnumerable<BannerResponse>>> GetBanners()
        {
            var banners = await _unitOfWork.GetRepository<Banner>().GetAllAsync();
            var count = await _unitOfWork.GetRepository<Banner>().CountAsync();
            if (count != 0)
            {
                return new Response<IEnumerable<BannerResponse>>(_mapper.Map<IEnumerable<BannerResponse>>(banners), message: "Success");
            }
            return new Response<IEnumerable<BannerResponse>>(message: "Empty");
        }

        public async Task<Response<IEnumerable<BannerResponse>>> GetBannersByDistributor(GetBannerByDistributorRequest request)
        {
            //list banner sort by position
            var banners = await _unitOfWork.GetRepository<Banner>().GetAsync(filter: x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)),
                                                                             orderBy: x => x.OrderBy(y => y.Position));
            var count = await _unitOfWork.GetRepository<Banner>().CountAsync(filter: x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)));
            if (count != 0)
            {
                var response = _mapper.Map<IEnumerable<BannerResponse>>(banners);
                return new Response<IEnumerable<BannerResponse>>(response, message: "Success");
            }
            return new Response<IEnumerable<BannerResponse>>(message: "Empty");
        }

        public async Task<Response<string>> UpdateBanner(UpdateBannerRequest request)
        {
            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Id))
                {
                    Banner newBanner = await _unitOfWork.GetRepository<Banner>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
                    var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(Guid.Parse(request.DistributorId));
                    if (distributor == null)
                    {
                        return new Response<string>(message: "Distributor is not existed ");
                    }
                    else if (distributor.IsActive)
                    {
                        if (request.Position < 0 || request.Position == 0 || request.Position > 5)
                        {
                            return new Response<string>(message: "Banner position must be from range 1 to 5");
                        }
                        else
                        {
                            newBanner.DistributorId = Guid.Parse(request.DistributorId);
                            newBanner.Name = request.Name;
                            newBanner.Link = request.Link;
                            newBanner.Image = request.Image;
                            var ExistedBanners = await _unitOfWork.GetRepository<Banner>().GetAsync(filter: x => x.DistributorId.Equals(newBanner.DistributorId),
                                                                                                    orderBy: x => x.OrderBy(y => y.Position));
                            foreach (Banner banner1 in ExistedBanners)
                            {
                                if (request.Position == banner1.Position && newBanner.Id != banner1.Id)
                                {
                                    request.Position = banner1.Position + 1;
                                }
                            }
                            newBanner.Position = request.Position;
                            newBanner.DateModified = DateTime.UtcNow;
                            _unitOfWork.GetRepository<Banner>().UpdateAsync(newBanner);
                            await _unitOfWork.SaveAsync();
                            return new Response<string>(newBanner.DistributorId.ToString(), message: "Banner is updated");
                        }
                    }
                    return new Response<string>(message: "Distributor is removed ");
                }
                return new Response<string>(message: "Banner ID can not be blanked");
            }
            return new Response<string>(message: "Fail to update banner");
        }
    }
}
