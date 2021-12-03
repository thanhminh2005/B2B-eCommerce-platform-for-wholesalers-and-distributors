using API.Contracts;
using API.Domains;
using API.DTOs.Products;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{

    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IProductService productService, IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        public IProductService IProductService
        {
            get => default;
            set
            {
            }
        }

        [HttpGet(ApiRoute.Products.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetProductByIdRequest request = new GetProductByIdRequest
            {
                Id = id
            };
            var response = await _productService.GetProductById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Products.GetDistributor)]
        public async Task<IActionResult> GetDistributor(string id)
        {
            GetProductByDistributorIdRequest request = new GetProductByDistributorIdRequest
            {
                DistributorId = id
            };
            var response = await _productService.GetProductByDistributorId(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Products.GetAllProductDistributor)]
        public async Task<IActionResult> GetAllProductDistributor(string DistributorId)
        {
            GetProductByDistributorIdRequest request = new GetProductByDistributorIdRequest
            {
                DistributorId = DistributorId
            };
            var response = await _productService.GetAllProductByDistributorId(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Products.RetailerGetDistributor)]
        public async Task<IActionResult> RetailerGetDistributor(string id)
        {
            GetProductByDistributorIdRequest request = new GetProductByDistributorIdRequest
            {
                DistributorId = id
            };
            var response = await _productService.RetailerGetProductByDistributorId(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Products.Filter)]
        public async Task<IActionResult> Filter([FromQuery] GetProductsWithFilterRequest request)
        {
            var response = await _productService.GetProductsWithFilter(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost(ApiRoute.Products.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var response = await _productService.CreateProduct(request);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut(ApiRoute.Products.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            Product OldProduct = await _unitOfWork.GetRepository<Product>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
            if (string.IsNullOrEmpty(request.SubCategoryId))
            {
                request.SubCategoryId = OldProduct.SubCategoryId.ToString();
            }
            if (string.IsNullOrEmpty(request.Name))
            {
                request.Name = OldProduct.Name;
            }
            if (string.IsNullOrEmpty(request.Image))
            {
                request.Image = OldProduct.Image;
            }
            if (request.Status == 0)
            {
                request.Status = OldProduct.Status;
            }
            if (string.IsNullOrEmpty(request.Description))
            {
                request.Description = OldProduct.Description;
            }
            var response = await _productService.UpdateProduct(request);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete(ApiRoute.Products.Delete)]
        public async Task<IActionResult> Delete([FromBody] RemoveProductRequest request)
        {

            var response = await _productService.RemoveProduct(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Products.Recommendation)]
        public async Task<IActionResult> Recommendation()
        {
            var response = await _productService.GetProductsRecommendation();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Products.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAll();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost(ApiRoute.Products.Upload)]
        public async Task<IActionResult> Upload(string distributorId, IFormFile file, CancellationToken cancellationToken)
        {
            var request = new CreateProductsUsingExcelRequest
            {
                DistributorId = distributorId,
                File = file,
            };
            var response = await _productService.ImportProductUsingExcel(request, cancellationToken);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
