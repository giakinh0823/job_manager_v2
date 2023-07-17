using BusinessObject;
using Client.Helper;
using client.Pages.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quartz;
using server.Dto.Auth;
using System.Net.Http.Headers;
using server.Dto.Base;
using client.Config.Security;

namespace Client.Pages.Scheduler
{
    [FptAuthorize] 
    public class IndexModel : PageModel
    {
        private readonly ServerConfig _serverConfig;
        private readonly ApiHelper _apiHelper;

        public IndexModel(ServerConfig serverConfig, ApiHelper apiHelper)
        {
            _serverConfig = serverConfig;
            _apiHelper = apiHelper;
        }

        public List<Job>? Jobs { get; set; } = default!;
        
        public async Task<IActionResult> OnGet()
        {
            ApiResponse<List<Job>> result = await _apiHelper.GetAsync<List<Job>>($"{_serverConfig.Domain}/api/v1/jobs", true);
            if (result.IsSuccess)
            {
                Jobs = result.Data;
            }
            else
            {
                string errorMessage = result.ErrorMessage;
                ViewData["Error"] = errorMessage;
            }
            return Page();
        }
        
        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id == null) return NotFound();
            await _apiHelper.DeleteAsync<BaseResponse>($"{_serverConfig.Domain}/api/v1/jobs/" + id, true);
            return new RedirectResult("/Scheduler");
        }
    }
}
