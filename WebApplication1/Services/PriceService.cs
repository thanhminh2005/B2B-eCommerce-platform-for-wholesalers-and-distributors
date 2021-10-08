﻿using API.Domains;
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
            if(price == null && count <= 5)
            {
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(request.ProductId));
                if(product != null)
                {
                    if (product.MinQuantity <= request.Volume)
                    {
                        var newPrice = _mapper.Map<Price>(request);
                        newPrice.Id = Guid.NewGuid();
                        newPrice.DateCreated = DateTime.UtcNow;
                        await _unitOfWork.GetRepository<Price>().AddAsync(newPrice);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(newPrice.Id.ToString(), message: "Price Added");
                    }
                }
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
            if(price != null)
            {
                price.Value = request.Value;
                price.Volume = request.Volume;
                price.DateModified = DateTime.UtcNow;
                _unitOfWork.GetRepository<Price>().UpdateAsync(price);
                await _unitOfWork.SaveAsync();
                return new Response<string>(price.Id.ToString(), message: "Updated");
            }
            return new Response<string>("Update Failed");
        }
    }
}