using client.Config.Security;
using client.Dto.Job;
using client.Dto.LogRes;
using client.Pages.Config;
using Client.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Scheduler
{
    [FptAuthorize]
    public class DetailsModel : PageModel
    {
        private readonly ServerConfig _serverConfig;
        private readonly ApiHelper _apiHelper;

        public DetailsModel(ServerConfig serverConfig, ApiHelper apiHelper)
        {
            _serverConfig = serverConfig;
            _apiHelper = apiHelper;
        }

        [BindProperty(Name = "id", SupportsGet = true)]
        public int? Id { get; set; } = default!;

        public JobResponse? Job { get; set; } = default!;
        public List<LogResponse>? Logs { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == null)
            {
                return NotFound();
            }

            ApiResponse<JobResponse> result = await _apiHelper.GetAsync<JobResponse>($"{_serverConfig.Domain}/api/v1/jobs/" + Id, true);
            if (result.IsSuccess)
            {
                Job = result.Data;
                if (Job == null)
                {
                    return NotFound();
                }
                Logs = Job.Logs.ToList();
            }
            else
            {
                string errorMessage = result.ErrorMessage;
                ViewData["Error"] = errorMessage;
            }

            return Page();
        }
    }
}
