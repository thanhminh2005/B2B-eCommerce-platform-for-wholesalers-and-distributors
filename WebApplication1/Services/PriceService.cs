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
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(request.ProductId));
            if(product != null)
            {
                if (count > 5)
                {
                    return new Response<string>("Product can only have 5 different prices at maximum");
                }
                else if (price == null && count <= 5)
                {
                    if (request.Volume > product.MinQuantity)
                    {
                        var newPrice = _mapper.Map<Price>(request);
                        newPrice.Id = Guid.NewGuid();
                        newPrice.DateCreated = DateTime.UtcNow;
                        product.DateModified = DateTime.UtcNow;
                        await _unitOfWork.GetRepository<Price>().AddAsync(newPrice);
                        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(product.Id.ToString(), message: "Price added successfully");
                    }
                    else if(request.Volume < product.MinQuantity)
                    {
                        var newPrice = _mapper.Map<Price>(request);
                        newPrice.Id = Guid.NewGuid();
                        newPrice.DateCreated = DateTime.UtcNow;
                        product.MinQuantity = newPrice.Volume;
                        product.DateModified = DateTime.UtcNow;
                        await _unitOfWork.GetRepository<Price>().AddAsync(newPrice);
                        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(product.Id.ToString(), message: "Price added successfully");
                    }
                }
                return new Response<string>(product.Id.ToString(),message: "This specific quantity already has a price, please try again!");
            }
            return new Response<string>("Product Id not found");
        }

        public async Task<Response<string>> DeletePrice(DeletePriceRequest request)
        {
            var price = await _unitOfWork.GetRepository<Price>().GetByIdAsync(Guid.Parse(request.Id));
            if (price != null)
            {
                var count = await _unitOfWork.GetRepository<Price>().CountAsync(x => x.ProductId.Equals(price.ProductId));
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(price.ProductId);
                if (count > 1)
                {
                    if(price.Volume > product.MinQuantity)
                    {
                        product.DateModified = DateTime.UtcNow;
                        _unitOfWork.GetRepository<Price>().DeleteAsync(price);
                        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(price.Id.ToString(), message: "Deleted price successfully");
                    }
                    else if(price.Volume == product.MinQuantity)
                    {
                        List<Price> listPrice = (List<Price>)await _unitOfWork.GetRepository<Price>().GetAsync(filter:x => x.ProductId.Equals(price.ProductId),
                                                                                            orderBy:x => x.OrderBy(y => y.Volume));
                        product.MinQuantity = listPrice[1].Volume;
                        product.DateModified = DateTime.UtcNow;
                        _unitOfWork.GetRepository<Price>().DeleteAsync(price);
                        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(price.Id.ToString(), message: "Deleted price successfully");
                    }
                    return new Response<string>(product.Id.ToString(), message: "Can not delete price with volume lower than Minimum Quantity of Product, please try again!");
                }
                return new Response<string>(product.Id.ToString(), message: "Can not delete the final price of product");
            }
            return new Response<string>("Price Id is not existed");
        }

        public async Task<Response<PriceResponse>> GetPriceById(GetPriceByIdRequest request)
        {
            var price = await _unitOfWork.GetRepository<Price>().GetByIdAsync(Guid.Parse(request.Id));

            if (price != null)
            {
                return new Response<PriceResponse>(_mapper.Map<PriceResponse>(price), message: "Succeed");
            }
            return new Response<PriceResponse>("Not Found");
        }

        public async Task<Response<IEnumerable<PriceResponse>>> GetPrices(GetPricesRequest request)
        {
            var prices = await _unitOfWork.GetRepository<Price>().GetAsync(x => x.ProductId.Equals(Guid.Parse(request.ProductId)), x => x.OrderBy(y => y.Volume));
            if (prices != null)
            {
                return new Response<IEnumerable<PriceResponse>>(_mapper.Map<IEnumerable<PriceResponse>>(prices), message: "Succeed");
            }
            return new Response<IEnumerable<PriceResponse>>("Not Found");
        }

        public async Task<Response<string>> UpdatePrice(UpdatePriceRequest request)
        {
            var price = await _unitOfWork.GetRepository<Price>().GetByIdAsync(Guid.Parse(request.Id));
            if (price != null)
            {
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(price.ProductId);
                if (request.Volume > product.MinQuantity)
                {
                    price.Value = request.Value;
                    price.Volume = request.Volume;
                    price.DateModified = DateTime.UtcNow;
                    product.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Price>().UpdateAsync(price);
                    _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(price.ProductId.ToString(), message: product.Name + "'s price updated successfully");
                }
                else if(request.Volume == product.MinQuantity)
                {
                    var oldPrice = await _unitOfWork.GetRepository<Price>().FirstAsync(x => x.ProductId.Equals(product.Id)
                                                                                 && x.Volume == request.Volume);
                    if(oldPrice.Id != price.Id)
                    {
                        _unitOfWork.GetRepository<Price>().DeleteAsync(oldPrice);
                    }
                    price.Value = request.Value;
                    price.Volume = request.Volume;
                    price.DateModified = DateTime.UtcNow;
                    product.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Price>().UpdateAsync(price);
                    _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(price.ProductId.ToString(), message: product.Name + "'s price updated successfully");
                }
                else
                {
                    price.Value = request.Value;
                    price.Volume = request.Volume;
                    price.DateModified = DateTime.UtcNow;
                    product.MinQuantity = price.Volume;
                    product.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Price>().UpdateAsync(price);
                    _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(price.ProductId.ToString(), message: product.Name + "'s price updated successfully");
                }
            }
            return new Response<string>(request.Id, "Price ID is not existed");
        }

    }
}
