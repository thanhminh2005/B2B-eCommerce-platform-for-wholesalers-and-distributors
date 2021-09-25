using API.DTOs.Products;
using API.Warppers;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IProductService
    {
        Task<Response<string>> CreateProduct(CreateProductRequest request);
        Task<Response<ProductResponse>> GetProductById(GetProductByIdRequest request);
    }
}
