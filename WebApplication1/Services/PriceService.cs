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
            var prices = await _unitOfWork.GetRepository<Price>().GetAsync(x => x.ProductId.Equals(Guid.Parse(request.ProductId)));
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(request.ProductId));
            if (product != null)
            {
                if (prices.Count() > 5)
                {
                    return new Response<string>("Out of bound", "Product can only have 5 different prices at maximum");
                }
                if (price == null && prices.Count() <= 5)
                {
                    var newPrice = _mapper.Map<Price>(request);
                    newPrice.Id = Guid.NewGuid();
                    newPrice.DateCreated = DateTime.UtcNow;
                    if (request.Volume < product.MinQuantity)
                    {
                        product.MinQuantity = request.Volume;
                        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                    }
                    await _unitOfWork.GetRepository<Price>().AddAsync(newPrice);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(newPrice.ProductId.ToString(), message: "Price added successfully");
                }
                return new Response<string>(price.ProductId.ToString(), message: "This specific quantity already has a price, please try again!");
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
                    if (price.Volume == product.MinQuantity)
                    {
                        var listPrice = await _unitOfWork.GetRepository<Price>().GetAsync(filter: x => x.ProductId.Equals(price.ProductId),
                                                                                            orderBy: x => x.OrderBy(y => y.Volume));
                        product.MinQuantity = listPrice.ToList().ElementAt(1).Volume;
                        product.DateModified = DateTime.UtcNow;
                        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                    }
                    _unitOfWork.GetRepository<Price>().DeleteAsync(price);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(price.ProductId.ToString(), message: "Deleted price successfully");
                }
                return new Response<string>(price.ProductId.ToString(), message: "Can not delete the final price of product");
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
                var prices = await _unitOfWork.GetRepository<Price>().GetAsync(x => x.ProductId.Equals(product.Id), orderBy: x => x.OrderBy(y => y.Volume));
                var pricesList = prices.ToList();
                if (!prices.Any(x => x.Volume == request.Volume && x.Value == request.Value))
                {
                    if (price.Volume == product.MinQuantity)
                    {
                        if (request.Volume < product.MinQuantity)
                        {
                            product.MinQuantity = request.Volume;
                        }
                        if (request.Volume > product.MinQuantity)
                        {
                            pricesList.Remove(price);
                            var minVolume = 0;
                            minVolume = pricesList.Min(x => x.Volume);
                            if (minVolume != 0)
                            {
                                product.MinQuantity = minVolume;
                            }
                        }
                    }
                    if (price.Volume > product.MinQuantity)
                    {
                        if (request.Volume != product.MinQuantity)
                        {
                            product.MinQuantity = request.Volume;
                        }
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
            }
            return new Response<string>(request.Id, "Price ID is not existed");
        }

    }
}
