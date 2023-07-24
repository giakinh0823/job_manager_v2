using BusinessObject;
using client.Config.Security;
using client.Pages.Config;
using Client.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using server.Dto.Job;

namespace Client.Pages.Scheduler
{
    [FptAuthorize]
    public class EditModel : PageModel
    {
        private readonly ServerConfig _serverConfig;
        private readonly ApiHelper _apiHelper;

        public EditModel(ServerConfig serverConfig, ApiHelper apiHelper)
        {
            _serverConfig = serverConfig;
            _apiHelper = apiHelper;
        }

        public JobResponse? Job { get; set; } = default!;

        [BindProperty]
        public JobUpdateRequest? JobUpdateRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }


            ApiResponse<JobResponse> result = await _apiHelper.GetAsync<JobResponse>($"{_serverConfig.Domain}/api/v1/jobs/" + id, true);
            if (result.IsSuccess)
            {
                Job = result.Data;
                if (Job == null)
                {
                    return NotFound();
                }
            }
            else
            {
                string errorMessage = result.ErrorMessage;
                ViewData["Error"] = errorMessage;
            }

            return Page();
        }

 
        public async Task<IActionResult> OnPostAsync()
        {
            if (JobUpdateRequest == null)
            {
                return Page();
            }

            ApiResponse<Job> result = await _apiHelper.PutAsync<Job>($"{_serverConfig.Domain}/api/v1/jobs", JobUpdateRequest, true);
            return RedirectToPage("./Index");
        }
    }
}
