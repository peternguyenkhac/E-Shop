using EShop.Libraries;
using EShop.Models;

namespace EShop.Services
{
    public class VnPayService
    {
        private readonly IConfiguration _configuration;

        private readonly OrderService _orderService;

        public VnPayService(IConfiguration configuration, OrderService orderService)
        {
            _configuration = configuration;
            _orderService = orderService;
        }

        public string CreatePaymentUrl(Order order, HttpContext context)
        {
            //Get Config Info
            string vnp_Returnurl = _configuration["Vnpay:Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = _configuration["Vnpay:BaseUrl"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _configuration["Vnpay:TmnCode"]; //Ma website
            string vnp_HashSecret = _configuration["Vnpay:HashSecret"]; //Chuoi bi mat

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Total * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.Id);
            vnpay.AddRequestData("vnp_OrderType", "Thanh toan");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString());

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }

        public VnPayLibrary PaymentExecute(IQueryCollection collection)
        {
            string returnContent = string.Empty;

            string vnp_HashSecret = _configuration["Vnpay:HashSecret"]; //Secret key
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (var (key, value) in collection)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }
            string vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                return vnpay;
            }
            return null;
        }
    }
}
