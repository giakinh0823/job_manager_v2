using Client.Helper;
using client.Pages.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using server.Dto.Payment;
using server.Dto.Job;
using client.Config.Security;

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

        var result = await _apiHelper.PostAsync<Object>($"{_serverConfig.Domain}/api/v1/payment", paymentRequest, true);
        if(result != null)
        {
            Object obj = result.Data;
            if(obj != null && obj is PaymentResponse)
            {
                PaymentResponse? paymentResponse = obj as PaymentResponse;
                return new RedirectResult(paymentResponse.Url);
            } 
        } else
        {
            string? errorMessage = result?.ErrorMessage;
            ViewData["Error"] = errorMessage;
        }

        return Page();
    }
}