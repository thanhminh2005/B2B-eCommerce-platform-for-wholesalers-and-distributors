using API.Contracts;
using API.Domains;
using API.DTOs.Banners;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        private readonly IUnitOfWork _unitOfWork;

        public BannerController(IBannerService bannerService, IUnitOfWork unitOfWork)
        {
            _bannerService = bannerService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost(ApiRoute.Banners.Create)]
        public async Task<IActionResult> Create([FromBody] CreateBannerRequest request)
        {
            var response = await _bannerService.CreateBanner(request);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Banners.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _bannerService.GetBanners();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Banners.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetBannerByIdRequest request = new GetBannerByIdRequest
            {
                Id = id
            };
            var response = await _bannerService.GetBannerById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Banners.GetDistributor)]
        public async Task<IActionResult> GetDistributor(string id)
        {
            GetBannerByDistributorRequest request = new GetBannerByDistributorRequest
            {
                DistributorId = id
            };
            var response = await _bannerService.GetBannersByDistributor(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.Banners.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateBannerRequest request)
        {
            Banner OldBanner = await _unitOfWork.GetRepository<Banner>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
            if (string.IsNullOrEmpty(request.DistributorId))
            {
                request.DistributorId = OldBanner.DistributorId.ToString();
            }
            if (string.IsNullOrEmpty(request.Name))
            {
                request.Name = OldBanner.Name;
            }
            if (string.IsNullOrEmpty(request.Link))
            {
                request.Link = OldBanner.Link;
            }
            if (string.IsNullOrEmpty(request.Image))
            {
                request.Image = OldBanner.Image;
            }
            if (request.Position == 0)
            {
                request.Position = OldBanner.Position;
            }
            var response = await _bannerService.UpdateBanner(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete(ApiRoute.Banners.Delete)]
        public async Task<IActionResult> Delete([FromBody] DeleteBannerRequest request)
        {
            var response = await _bannerService.DeleteBanner(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
