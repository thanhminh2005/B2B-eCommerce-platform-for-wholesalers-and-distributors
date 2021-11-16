using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace API.MoMo
{
    public class MoMoHandler
    {
        private readonly IHttpContextAccessor _httpContext;

        public MoMoHandler(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public MoMoPaymentResponse CreatePayment(MoMoPaymentRequest request, IConfiguration configuration)
        {
            string endpoint = MoMoEndpoint.PaymentCreate;
            string secretkey = configuration.GetSection("MomoKey")["SecretKey"];
            string partnerCode = configuration.GetSection("MomoKey")["PartnerCode"];
            string accessKey = configuration.GetSection("MomoKey")["AccessKey"];
            string requestType = configuration.GetSection("MomoKey")["RequestType"];
            string lang = configuration.GetSection("MomoKey")["Lang"];
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";
            var domainName = _httpContext.HttpContext.Request.Scheme + Uri.SchemeDelimiter + _httpContext.HttpContext.Request.Host;
            string ipnUrl = domainName + "/" + Contracts.ApiRoute.Momo.IPN;
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + request.Amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + request.OrderId +
                "&orderInfo=" + request.OrderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + request.RedirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;

            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, secretkey);

            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", request.Amount },
                { "orderId", request.OrderId },
                { "orderInfo", request.OrderInfo },
                { "redirectUrl", request.RedirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", lang },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            string responseFromMomo = MoMoPayment.SendPaymentRequest(endpoint, message.ToString());

            MoMoPaymentResponse response = JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseFromMomo);

            if (response.ResultCode == 0)
            {
                return response;
            }
            return null;
        }

        public RefundResponse RefundPayment(RefundRequest request, IConfiguration configuration)
        {
            string endpoint = MoMoEndpoint.Refund;
            string secretkey = configuration.GetSection("MomoKey")["SecretKey"];
            string partnerCode = configuration.GetSection("MomoKey")["PartnerCode"];
            string accessKey = configuration.GetSection("MomoKey")["AccessKey"];
            string lang = configuration.GetSection("MomoKey")["Lang"];
            string description = "";
            string requestId = request.RequestId;
            string refundHash = "accessKey=" + accessKey +
                                                    "&amount=" + request.Amount +
                                                    "&description" + description +
                                                    "&orderId=" + request.OrderId +
                                                    "&partnerCode=" + request.PartnerCode +
                                                    "&requestId=" + request.RequestId +
                                                    "&transId=" + request.TransId
                                                    ;

            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(refundHash, secretkey);

            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "orderId", request.OrderId },
                { "requestId", requestId },
                { "amount", request.Amount },
                { "transId", request.TransId },
                { "lang", lang },
                { "description", description },
                { "signature", signature }

            };
            string responseFromMomo = MoMoPayment.SendPaymentRequest(endpoint, message.ToString());

            RefundResponse response = JsonConvert.DeserializeObject<RefundResponse>(responseFromMomo);

            if (response.ResultCode == 0)
            {
                return response;
            }
            return null;
        }
    }
}
