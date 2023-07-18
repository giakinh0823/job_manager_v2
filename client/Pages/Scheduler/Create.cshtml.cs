using Client.Helper;
using client.Pages.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quartz;
using server.Dto.Job;
using BusinessObject;
using client.Config.Security;

namespace Client.Pages.Scheduler
{
    [FptAuthorize]
    public class CreateModel : PageModel
    {
        private readonly ServerConfig _serverConfig;
        private readonly ApiHelper _apiHelper;

        public CreateModel(ServerConfig serverConfig, ApiHelper apiHelper)
        {
            _serverConfig = serverConfig;
            _apiHelper = apiHelper;
        }


        [BindProperty]
        public JobCreateRequest JobCreateRequest { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        
        public async Task<IActionResult> OnPost()
        {
            if(JobCreateRequest == null)
            {
                return Page();
            }

            ApiResponse<Object> result = await _apiHelper.PostAsync<Object>($"{_serverConfig.Domain}/api/v1/jobs", JobCreateRequest, true);
            if (result != null || result.IsSuccess)
            {
                string? errorMessage = result?.ErrorMessage;
                TempData["Error"] = errorMessage;
            }

            return new RedirectResult("/Scheduler");
        }
    }
}
