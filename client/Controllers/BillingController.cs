using BusinessObject;
using client.Pages.Config;
using Client.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Constant;
using server.Dto.Auth;
using server.Dto.Payment;

namespace client.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : Controller
    {
        private readonly ServerConfig _serverConfig;
        private readonly ApiHelper _apiHelper;

        public BillingController(ServerConfig serverConfig, ApiHelper apiHelper)
        {
            _serverConfig = serverConfig;
            _apiHelper = apiHelper;
        }

        [HttpGet("success/{id}")]
        public async Task<IActionResult> BillingSuccess(int id)
        {
            ApiResponse<PaymentResponse> apiResponse = await _apiHelper.GetAsync<PaymentResponse>($"{_serverConfig.Domain}/api/v1/payment/success/" + id, true);
            return Redirect("/settings/billing");
        }

        [HttpGet("cancel/{id}")]
        public async Task<IActionResult> BillingCancel(int id)
        {
            ApiResponse<PaymentResponse> apiResponse = await _apiHelper.GetAsync<PaymentResponse>($"{_serverConfig.Domain}/api/v1/payment/cancel/" + id, true);
            return Redirect("/settings/billing");
        }
    }
}
