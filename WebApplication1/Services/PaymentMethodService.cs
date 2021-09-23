using API.Domains;
using API.DTOs.PaymentMethods;
using API.Interfaces;
using API.Repositories;
using API.Warppers;
using AutoMapper;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentMethodService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreatePaymentMethod(CreatePaymentMethodRequest request)
        {
            var payment = await _unitOfWork.GetRepository<PaymentMethod>().FirstAsync(x => x.Name.Equals(request.Name));
            if(payment == null)
            {
                var newPayment = _mapper.Map<PaymentMethod>(request);
                newPayment.Id = Guid.NewGuid();
                newPayment.DateCreated = DateTime.UtcNow;
                await _unitOfWork.GetRepository<PaymentMethod>().AddAsync(newPayment);
                await _unitOfWork.SaveAsync();
                return new Response<string>(request.Name, message: "Payment Method Created");
            }
            return new Response<string>(message: "Failed to create Payment Method");
        }

        public async Task<Response<string>> DeletePaymentMethod(DeletePaymentMethodRequest request)
        {
            var payment = await _unitOfWork.GetRepository<PaymentMethod>().GetByIdAsync(Guid.Parse(request.Id));
            if(payment != null)
            {
                var inUsed = await _unitOfWork.GetRepository<RetailerPaymentMethod>().FirstAsync(x => x.PaymentMethodId.Equals(Guid.Parse(request.Id)));
                if(inUsed == null)
                {
                    _unitOfWork.GetRepository<PaymentMethod>().DeleteAsync(payment);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(payment.Name, message: "Payment Method Deleted");
                } 
            }
            return new Response<string>(message: "Failed to Delete PaymentMethod");
        }

        public async Task<Response<PaymentMethodResponse>> GetPaymentMethodById(GetPaymentMethodByIdRequest request)
        {
            var payment = await _unitOfWork.GetRepository<PaymentMethod>().GetByIdAsync(Guid.Parse(request.Id));
            if(payment != null)
            {
                return new Response<PaymentMethodResponse>(_mapper.Map<PaymentMethodResponse>(payment), message: "Succeed");
            }
            return new Response<PaymentMethodResponse>(message: "Failed");
        }

        public async Task<Response<IEnumerable<PaymentMethodResponse>>> GetPaymentMethods()
        {
            var payment = await _unitOfWork.GetRepository<PaymentMethod>().GetAllAsync();
            if (payment != null)
            {
                return new Response<IEnumerable<PaymentMethodResponse>>(_mapper.Map< IEnumerable < PaymentMethodResponse >> (payment), message: "Succeed");
            }
            return new Response<IEnumerable<PaymentMethodResponse>>(message: "Failed");
        }
        public async Task<Response<string>> UpdatePaymentMethod(UpdatePaymentMethodRequest request)
        {
            var payment = await _unitOfWork.GetRepository<PaymentMethod>().GetByIdAsync(Guid.Parse(request.Id));
            if (payment != null)
            {
                payment.DateModified = DateTime.UtcNow;
                payment.Description = request.Description;
                payment.Name = request.Name;
                _unitOfWork.GetRepository<PaymentMethod>().UpdateAsync(payment);
                await _unitOfWork.SaveAsync();
                return new Response<string>(payment.Name, message: "Payment Method Updated");
            }
            return new Response<string>(message: "Failed to Update PaymentMethod");
        }

    }
}
