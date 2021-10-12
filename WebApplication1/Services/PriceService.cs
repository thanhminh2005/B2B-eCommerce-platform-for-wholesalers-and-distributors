using API.Domains;
using API.DTOs.Prices;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class PriceService : IPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PriceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreatePrice(CreatePriceRequest request)
        {
            var price = await _unitOfWork.GetRepository<Price>().FirstAsync(x => x.ProductId.Equals(Guid.Parse(request.ProductId))
                                                                                 && x.Volume == request.Volume);
            var count = await _unitOfWork.GetRepository<Price>().CountAsync(x => x.ProductId.Equals(Guid.Parse(request.ProductId)));
            if(count > 5)
            {
                return new Response<string>("Product can only have 5 different prices at maximum");
            }
            else if(price == null && count <= 5)
            {
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(request.ProductId));
                if(product != null)
                {
                    if (product.MinQuantity <= request.Volume)
                    {
                        var newPrice = _mapper.Map<Price>(request);
                        newPrice.Id = Guid.NewGuid();
                        newPrice.DateCreated = DateTime.UtcNow;
                        product.DateModified = DateTime.UtcNow;
                        await _unitOfWork.GetRepository<Price>().AddAsync(newPrice);
                        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(product.Name, message: "Price added successfully");
                    }
                    return new Response<string>("The minimum volume required is more than: " + product.MinQuantity);
                }
                return new Response<string>("Product Id not found");
            }
            return new Response<string>("Add Failed");
        }

        public async Task<Response<string>> DeletePrice(DeletePriceRequest request)
        {
            var price = await _unitOfWork.GetRepository<Price>().GetByIdAsync(Guid.Parse(request.Id));
            if(price != null)
            {
                _unitOfWork.GetRepository<Price>().DeleteAsync(price);
                await _unitOfWork.SaveAsync();
                return new Response<string>(price.Id.ToString(), message: "Succeed");
            }
            return new Response<string>("Delete Failed");
        }

        public async Task<Response<PriceResponse>> GetPriceById(GetPriceByIdRequest request)
        {
            var price = await _unitOfWork.GetRepository<Price>().GetByIdAsync(Guid.Parse(request.Id));

            if(price != null)
            {
                return new Response<PriceResponse>(_mapper.Map<PriceResponse>(price), message: "Succeed");
            }
            return new Response<PriceResponse>("Not Found");
        } 

        public async Task<Response<IEnumerable<PriceResponse>>> GetPrices(GetPricesRequest request)
        {
            var prices = await _unitOfWork.GetRepository<Price>().GetAsync(x => x.ProductId.Equals(Guid.Parse(request.ProductId)));

            if (prices != null)
            {
                return new Response<IEnumerable<PriceResponse>>(_mapper.Map<IEnumerable<PriceResponse>>(prices), message: "Succeed");
            }
            return new Response<IEnumerable<PriceResponse>>("Not Found");
        }

        public async Task<Response<string>> UpdatePrice(UpdatePriceRequest request)
        {
            var price = await _unitOfWork.GetRepository<Price>().GetByIdAsync(Guid.Parse(request.Id));
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(price.ProductId);
            if(price != null)
            {
                if (product.MinQuantity <= request.Volume)
                {
                    price.Value = request.Value;
                    price.Volume = request.Volume;
                    price.DateModified = DateTime.UtcNow;
                    product.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Price>().UpdateAsync(price);
                    _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(price.Id.ToString(), message: "Updated successfully");
                }
                return new Response<string>("The minimum volume required is more than: " + product.MinQuantity);
            }
            return new Response<string>("Update Failed");
        }
    }
}
