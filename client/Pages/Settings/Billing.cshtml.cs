using client.Config.Security;
using client.Dto.Payment;
using client.Pages.Config;
using Client.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Settings;

[FptAuthorize]
public class BillingModel : PageModel
{

    private readonly ServerConfig _serverConfig;
    private readonly ApiHelper _apiHelper;

    public BillingModel(ServerConfig serverConfig, ApiHelper apiHelper)
    {
        _serverConfig = serverConfig;
        _apiHelper = apiHelper;
    }

    public async Task<IActionResult> OnGet()
    {
        if (TempData["Error"] is string errorMessage)
        {
            ViewData["Error"] = errorMessage;
        }

        ViewData["paymentInfo"] = "You're on Free Plan";
        ViewData["numberOfSchedulers"] = "1 Scheduler";

        ApiResponse<PaymentResponse> result = await _apiHelper.GetAsync<PaymentResponse>($"{_serverConfig.Domain}/api/v1/payment", true);

        if (result != null)
        {
            PaymentResponse paymentResponse = result.Data;
            if(paymentResponse != null)
            {
                ViewData["paymentInfo"] = paymentResponse.PaymentInfo;
                ViewData["numberOfSchedulers"] = paymentResponse.NumberOfSchedulers;
            }
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPost(int? month)
    {
        if (month == null) return Page();

        PaymentRequest paymentRequest = new PaymentRequest()
        {
            Month = (int)month
        };

        var result = await _apiHelper.PostAsync<PaymentResponse>($"{_serverConfig.Domain}/api/v1/payment", paymentRequest, true);
        if(result != null)
        {
            if(result.IsSuccess && result.Data != null && result.Data.Url != null)
            {
                PaymentResponse? paymentResponse = result.Data;
                return new RedirectResult(paymentResponse?.Url);
            } else
            {
                string? errorMessage = result?.ErrorMessage;
                TempData["Error"] = errorMessage;
            }
        } else
        {
            string? errorMessage = result?.ErrorMessage;
            ViewData["Error"] = errorMessage;
        }

        ViewData["paymentInfo"] = "You're on Free Plan";
        ViewData["numberOfSchedulers"] = "1 Scheduler";

        ApiResponse<PaymentResponse> resultPayment = await _apiHelper.GetAsync<PaymentResponse>($"{_serverConfig.Domain}/api/v1/payment", true);

        if (resultPayment != null)
        {
            PaymentResponse paymentResponse = resultPayment.Data;
            if (paymentResponse != null)
            {
                ViewData["paymentInfo"] = paymentResponse.PaymentInfo;
                ViewData["numberOfSchedulers"] = paymentResponse.NumberOfSchedulers;
            }
        }

        return Page();
    }
}