using Client.Helper;
using client.Pages.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using prn_job_manager.Models;
using server.Dto.Auth;
using BusinessObject;
using client.Config;

namespace Client.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly ServerConfig _serverConfig;
        private readonly ApiHelper _apiHelper;

        public RegisterModel(ServerConfig serverConfig, ApiHelper apiHelper)
        {
            _serverConfig = serverConfig;
            _apiHelper = apiHelper;

        }
        [BindProperty]
        public RegisterRequest? RegisterRequest { get; set; }
        
        public IActionResult OnGet()
        {
            
            if (HttpContext.Session.GetString("email") != null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (RegisterRequest == null)
            {
                return Page();
            }
            if (ModelState.IsValid)
            {
                ApiResponse<User> apiResponse = await _apiHelper.PostAsync<User>($"{_serverConfig.Domain}/signup", RegisterRequest);

                if (apiResponse.IsSuccess)
                {
                    return RedirectToPage("/Auth/Login");
                }
                else
                {
                    string errorMessage = apiResponse.ErrorMessage;
                    ViewData["Error"] = errorMessage;
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }
    }

}
