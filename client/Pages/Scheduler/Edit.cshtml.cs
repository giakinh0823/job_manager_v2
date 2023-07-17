using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Helper;
using client.Pages.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prn_job_manager.Models;
using Quartz;
using server.Dto.Job;
using BusinessObject;

namespace Client.Pages.Scheduler
{
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
