using API.Domains;
using API.DTOs.RetailerPaymentMethods;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class RetailerPaymentMethodService : IRetailerPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RetailerPaymentMethodService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateRetailerPaymentMethod(CreateRetailerPaymentMethodRequest request)
        {
            var payment = new RetailerPaymentMethod();
            payment = _mapper.Map<RetailerPaymentMethod>(request);
            payment.Id = Guid.NewGuid();
            payment.DateCreated = DateTime.UtcNow;
            await _unitOfWork.GetRepository<RetailerPaymentMethod>().AddAsync(payment);
            await _unitOfWork.SaveAsync();
            return new Response<string>(payment.Id.ToString(), message: "Created");
        }

        public async Task<Response<string>> DeleteRetailerPaymentMethod(DeleteRetailerPaymentMethodRequest request)
        {
            var payment = await _unitOfWork.GetRepository<RetailerPaymentMethod>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
            if (payment != null)
            {
                _unitOfWork.GetRepository<RetailerPaymentMethod>().DeleteAsync(payment);
                await _unitOfWork.SaveAsync();
                return new Response<string>(request.Id, message: "Deleted");
            }
            return new Response<string>(message: "Failed");
        }

        public async Task<Response<RetailerPaymentMethodResponse>> GetRetailerPaymentMethodById(GetRetailerPaymentMethodByIdRequest request)
        {
            var payment = await _unitOfWork.GetRepository<RetailerPaymentMethod>().GetByIdAsync(Guid.Parse(request.Id));
            if (payment != null)
            {
                return new Response<RetailerPaymentMethodResponse>(_mapper.Map<RetailerPaymentMethodResponse>(payment), message: "Succeed");
            }
            return new Response<RetailerPaymentMethodResponse>(message: "Not found");
        }

        public async Task<Response<IEnumerable<RetailerPaymentMethodResponse>>> GetRetailerPaymentMethods(GetRetailerPaymentMethodsRequest request)
        {
            var payments = await _unitOfWork.GetRepository<RetailerPaymentMethod>().GetAllAsync();
            if (!string.IsNullOrWhiteSpace(request.RetailerId))
            {
                payments = payments.Where(x => x.RetailerId.Equals(Guid.Parse(request.RetailerId)));
            }
            if (!string.IsNullOrWhiteSpace(request.PaymentMethodId))
            {
                payments = payments.Where(x => x.PaymentMethodId.Equals(Guid.Parse(request.PaymentMethodId)));
            }
            if (payments.Count() != 0)
            {
                return new Response<IEnumerable<RetailerPaymentMethodResponse>>(_mapper.Map<IEnumerable<RetailerPaymentMethodResponse>>(payments), message: "Succeed");
            }
            return new Response<IEnumerable<RetailerPaymentMethodResponse>>(message: "Empty");

        }

        public async Task<Response<string>> UpdateRetailerPaymentMethod(UpdateRetailerPaymentMethodRequest request)
        {
            var payment = await _unitOfWork.GetRepository<RetailerPaymentMethod>().GetByIdAsync(Guid.Parse(request.Id));
            if (payment != null)
            {
                payment.PaymentMethodId = Guid.Parse(request.PaymentMethodId);
                payment.Detail = request.Detail;
                payment.DateModified = DateTime.UtcNow;
                _unitOfWork.GetRepository<RetailerPaymentMethod>().UpdateAsync(payment);
                await _unitOfWork.SaveAsync();
                return new Response<string>(payment.Id.ToString(), message: "Updated");
            }
            return new Response<string>(message: "Failed");
        }
    }
}
