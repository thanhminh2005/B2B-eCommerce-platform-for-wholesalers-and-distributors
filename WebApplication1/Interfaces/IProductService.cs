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
        Task<Response<IEnumerable<ProductResponse>>> GetProductByDistributorId(GetProductByDistributorIdRequest request);
        Task<PagedResponse<IEnumerable<ProductResponse>>> GetProductsWithFilter(GetProductsWithFilterRequest request);

    }
}
