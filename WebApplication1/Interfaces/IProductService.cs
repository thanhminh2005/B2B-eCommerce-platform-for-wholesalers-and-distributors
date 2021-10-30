using API.DTOs.Products;
using API.Warppers;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IProductService
    {
        Task<Response<string>> CreateProduct(CreateProductRequest request);
        Task<Response<ProductResponse>> GetProductById(GetProductByIdRequest request);
        Task<Response<string>> UpdateProduct(UpdateProductRequest request);
        Task<Response<string>> RemoveProduct(RemoveProductRequest request);
        Task<PagedResponse<IEnumerable<ProductResponse>>> GetProductByDistributorId(GetProductByDistributorIdRequest request);
        Task<Response<IEnumerable<ProductResponse>>> GetAllProductByDistributorId(GetProductByDistributorIdRequest request);
        Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> RetailerGetProductByDistributorId(GetProductByDistributorIdRequest request);
        Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetProductsWithFilter(GetProductsWithFilterRequest request);
        Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetProductsRecommendation();
        Task<PagedResponse<IEnumerable<ProductResponse>>> GetAll();
        Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetProductsByCategory(GetProductByCategoryIdRequest request);
        Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetPopularProductsBySubCategory(GetProductByCategoryIdRequest request);
    }
}
