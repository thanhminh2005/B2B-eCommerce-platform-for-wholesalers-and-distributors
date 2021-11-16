using API.Contracts;
using API.Domains;
using API.DTOs.HomeBanners;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class HomeBannerController : ControllerBase
    {
        private readonly IHomeBannerService _homeHomeBannerService;
        private readonly IUnitOfWork _unitOfWork;

        public HomeBannerController(IHomeBannerService homeHomeBannerService, IUnitOfWork unitOfWork)
        {
            _homeHomeBannerService = homeHomeBannerService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost(ApiRoute.HomeBanners.Create)]
        public async Task<IActionResult> Create([FromBody] CreateHomeBannerRequest request)
        {
            var response = await _homeHomeBannerService.CreateHomeBanner(request);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.HomeBanners.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _homeHomeBannerService.GetHomeBanners();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.HomeBanners.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetHomeBannerByIdRequest request = new GetHomeBannerByIdRequest
            {
                Id = id
            };
            var response = await _homeHomeBannerService.GetHomeBannerById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.HomeBanners.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateHomeBannerRequest request)
        {
            HomeBanner OldHomeBanner = await _unitOfWork.GetRepository<HomeBanner>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
            if (string.IsNullOrEmpty(request.Name))
            {
                request.Name = OldHomeBanner.Name;
            }
            if (string.IsNullOrEmpty(request.Link))
            {
                request.Link = OldHomeBanner.Link;
            }
            if (string.IsNullOrEmpty(request.Image))
            {
                request.Image = OldHomeBanner.Image;
            }
            if (request.Position == 0)
            {
                request.Position = OldHomeBanner.Position;
            }
            var response = await _homeHomeBannerService.UpdateHomeBanner(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete(ApiRoute.HomeBanners.Delete)]
        public async Task<IActionResult> Delete([FromBody] DeleteHomeBannerRequest request)
        {
            var response = await _homeHomeBannerService.DeleteHomeBanner(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
