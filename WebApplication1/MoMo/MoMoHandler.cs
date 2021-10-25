using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace API.MoMo
{
    public class MoMoHandler
    {
        public static MoMoPaymentResponse CreatePayment(MoMoPaymentRequest request, IConfiguration configuration)
        {
            string endpoint = MoMoEndpoint.PaymentCreate;
            string secretkey = configuration.GetSection("MomoKey")["SecretKey"];
            string partnerCode = configuration.GetSection("MomoKey")["PartnerCode"];
            string accessKey = configuration.GetSection("MomoKey")["AccessKey"];
            string requestType = configuration.GetSection("MomoKey")["RequestType"];
            string lang = configuration.GetSection("MomoKey")["Lang"];
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";
            string ipnUrl = Contracts.ApiRoute.Momo.IPN;
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
            string responseFromMomo = MoMoPayment.sendPaymentRequest(endpoint, message.ToString());

            MoMoPaymentResponse response = JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseFromMomo);

            if (response.ResultCode == 0)
            {
                return response;
            }
            return null;
        }
    }
}
