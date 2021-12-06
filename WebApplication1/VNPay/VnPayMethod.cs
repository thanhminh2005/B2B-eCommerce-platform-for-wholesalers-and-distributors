using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace API.VNPay
{
    public class VnPayMethod
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public VnPayMethod(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
        }

        public string CreatePayUrl(VnPayOrderInfo orderInfo)
        {
            var result = "";
            string ReturnUrl = orderInfo.ReturnUrl;
            string PayUrl = _configuration.GetSection("VNPay")["PayUrl"];
            string TmnCode = _configuration.GetSection("VNPay")["TmnCode"];
            string HashSecret = _configuration.GetSection("VNPay")["HashSecret"];
            if (string.IsNullOrEmpty(TmnCode) || string.IsNullOrEmpty(HashSecret))
            {
                return result;
            }
            VnPayLibrary vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", TmnCode);
            vnPay.AddRequestData("vnp_Amount", (orderInfo.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnPay.AddRequestData("vnp_BankCode", orderInfo.BankCode);
            vnPay.AddRequestData("vnp_CreateDate", orderInfo.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContext));
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_OrderInfo", orderInfo.OrderDesc.ToString());
            vnPay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnPay.AddRequestData("vnp_ReturnUrl", ReturnUrl);
            vnPay.AddRequestData("vnp_TxnRef", orderInfo.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            result = vnPay.CreateRequestUrl(PayUrl, HashSecret);
            return result;
        }


    }
}
