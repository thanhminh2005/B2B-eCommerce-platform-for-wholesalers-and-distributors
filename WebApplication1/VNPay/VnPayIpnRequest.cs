﻿namespace API.VNPay
{
    public class VnPayIpnRequest
    {
        public string Vnp_TmnCode { get; set; }
        public long Vnp_Amount { get; set; }
        public string Vnp_BankCode { get; set; }
        public string Vnp_BankTranNo { get; set; }
        public string Vnp_CardType { get; set; }
        public string Vnp_PayDate { get; set; }
        public string Vnp_OrderInfo { get; set; }
        public long Vnp_TransactionNo { get; set; }
        public string Vnp_ResponseCode { get; set; }
        public string Vnp_TransactionStatus { get; set; }
        public long Vnp_TxnRef { get; set; }
        public string Vnp_SecureHashType { get; set; }
        public string Vnp_SecureHash { get; set; }
    }
}
