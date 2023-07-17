using Client.Helper;
using client.Pages.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quartz;
using server.Dto.Job;
using BusinessObject;

namespace Client.Pages.Scheduler
{
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

            ApiResponse<Job> result = await _apiHelper.PostAsync<Job>($"{_serverConfig.Domain}/api/v1/jobs", JobCreateRequest, true);

            return new RedirectResult("/Scheduler");
        }
    }
}
