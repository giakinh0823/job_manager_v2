using Client.Helper;
using client.Pages.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using server.Dto.Payment;
using server.Dto.Job;

namespace Client.Pages.Settings;

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
        var email = HttpContext.Session.GetString("email");
        if (email == null)
        {
            Response.Redirect("/auth/login");
        }
        
        ViewData["paymentInfo"] = "You're on Free Plan";
        ViewData["numberOfSchedulers"] = "1 Scheduler";

        ApiResponse<PaymentResponse> result = await _apiHelper.GetAsync<PaymentResponse>($"{_serverConfig.Domain}/api/v1/payment", true);

        if (result != null)
        {
            PaymentResponse paymentResponse = result.Data;
            ViewData["paymentInfo"] = paymentResponse.PaymentInfo;
            ViewData["numberOfSchedulers"] = paymentResponse.NumberOfSchedulers;
        }

        return Page();
    }
    
    public async Task<RedirectResult> OnPost(int? month)
    {
        if (month == null) return new RedirectResult("/settings/billing");

        PaymentRequest paymentRequest = new PaymentRequest()
        {
            Month = (int)month
        };

        ApiResponse<PaymentResponse> result = await _apiHelper.PostAsync<PaymentResponse>($"{_serverConfig.Domain}/api/v1/payment", paymentRequest, true);
        if(result != null)
        {
            PaymentResponse paymentResponse = result.Data;
            if(paymentResponse != null)
            {
                return new RedirectResult(paymentResponse.Url);
            }
        }

        return new RedirectResult("/settings/billing");
    }
}