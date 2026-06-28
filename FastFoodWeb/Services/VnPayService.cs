using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace FastFoodWeb.Services;

/// <summary>
/// Xây dựng URL thanh toán VNPay và xác thực kết quả trả về.
/// Tài liệu: https://sandbox.vnpayment.vn/apis/docs/thanh-toan-pay/pay.md
/// </summary>
public class VnPayService
{
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VnPayService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }

    // ------------------------------------------------------------------ //
    //  TẠO URL THANH TOÁN
    // ------------------------------------------------------------------ //
    public string CreatePaymentUrl(int orderId, decimal amount, string orderInfo)
    {
        var vnp = _config.GetSection("VNPay");
        string tmnCode   = vnp["TmnCode"]!;
        string hashSecret = vnp["HashSecret"]!;
        string baseUrl   = vnp["BaseUrl"]!;
        string returnUrl = vnp["ReturnUrl"]!;

        var request = _httpContextAccessor.HttpContext!.Request;
        string clientIp = request.Headers["X-Forwarded-For"].FirstOrDefault()
                       ?? request.HttpContext.Connection.RemoteIpAddress?.ToString()
                       ?? "127.0.0.1";

        // Chỉ lấy IPv4 nếu có nhiều IP
        if (clientIp.Contains(','))
            clientIp = clientIp.Split(',')[0].Trim();

        // VNPay yêu cầu IPv4
        if (clientIp == "::1") clientIp = "127.0.0.1";

        string txnRef   = $"{orderId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        string createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
        string expireDate = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");

        // Số tiền VNPay tính theo đơn vị VNĐ × 100
        long vnpAmount = (long)(amount * 100);

        var param = new SortedDictionary<string, string>
        {
            ["vnp_Version"]    = "2.1.0",
            ["vnp_Command"]    = "pay",
            ["vnp_TmnCode"]    = tmnCode,
            ["vnp_Amount"]     = vnpAmount.ToString(),
            ["vnp_CurrCode"]   = "VND",
            ["vnp_TxnRef"]     = txnRef,
            ["vnp_OrderInfo"]  = orderInfo,
            ["vnp_OrderType"]  = "other",
            ["vnp_Locale"]     = "vn",
            ["vnp_ReturnUrl"]  = returnUrl,
            ["vnp_IpAddr"]     = clientIp,
            ["vnp_CreateDate"] = createDate,
            ["vnp_ExpireDate"] = expireDate,
        };

        string queryString = BuildQueryString(param);
        string signature   = HmacSha512(hashSecret, queryString);

        return $"{baseUrl}?{queryString}&vnp_SecureHash={signature}";
    }

    // ------------------------------------------------------------------ //
    //  XÁC THỰC KẾT QUẢ TRẢ VỀ
    // ------------------------------------------------------------------ //
    public (bool isValid, bool isPaid, string txnRef, string responseCode) ValidateReturn(IQueryCollection query)
    {
        string hashSecret = _config["VNPay:HashSecret"]!;

        string receivedHash = query["vnp_SecureHash"].ToString();
        string responseCode = query["vnp_ResponseCode"].ToString();
        string txnRef       = query["vnp_TxnRef"].ToString();

        // Tái tạo tập tham số (bỏ vnp_SecureHash)
        var param = new SortedDictionary<string, string>();
        foreach (var kv in query)
        {
            if (kv.Key == "vnp_SecureHash" || kv.Key == "vnp_SecureHashType")
                continue;
            param[kv.Key] = kv.Value.ToString();
        }

        string queryString  = BuildQueryString(param);
        string expectedHash = HmacSha512(hashSecret, queryString);

        bool isValid = string.Equals(expectedHash, receivedHash, StringComparison.OrdinalIgnoreCase);
        bool isPaid  = isValid && responseCode == "00";

        return (isValid, isPaid, txnRef, responseCode);
    }

    // ------------------------------------------------------------------ //
    //  HELPERS
    // ------------------------------------------------------------------ //
    private static string BuildQueryString(SortedDictionary<string, string> param)
    {
        var sb = new StringBuilder();
        foreach (var kv in param)
        {
            if (!string.IsNullOrEmpty(kv.Value))
            {
                if (sb.Length > 0) sb.Append('&');
                sb.Append(WebUtility.UrlEncode(kv.Key));
                sb.Append('=');
                sb.Append(WebUtility.UrlEncode(kv.Value));
            }
        }
        return sb.ToString();
    }

    private static string HmacSha512(string key, string data)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToHexString(hash).ToLower();
    }
}
