using System.Security.Claims;
using client.Config;
using client.Dto.Auth;
using client.Pages.Config;
using Client.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Auth
{
    public class LoginModel : PageModel
    {

        private readonly ServerConfig _serverConfig;
        private readonly ApiHelper _apiHelper;

        [BindProperty]
        public LoginRequest? LoginRequest { get; set; }

        public LoginModel(ServerConfig serverConfig, ApiHelper apiHelper)
        {
            _serverConfig = serverConfig;
            _apiHelper = apiHelper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (LoginRequest == null)
            {
                return Page();
            }

            if (ModelState.IsValid)
            {
                ApiResponse<AccessTokenResponse> apiResponse = await _apiHelper.PostAsync<AccessTokenResponse>($"{_serverConfig.Domain}/login", LoginRequest);

                if (apiResponse.IsSuccess)
                {
                    AccessTokenResponse? accessToken = apiResponse.Data;
                    HttpContext.Session.SetString("AccessToken", accessToken.AccessToken);
                    HttpContext.Session.SetString("RefreshToken", accessToken.RefreshToken);
                    return RedirectToPage("/scheduler/index");
                }
                else
                {
                    string errorMessage = apiResponse.ErrorMessage;
                    ViewData["Error"] = errorMessage;
                    return Page();
                }
            } else
            {
                return Page();
            }  
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Remove("AccessToken");
            HttpContext.Session.Remove("RefreshToken");
            return RedirectToPage("/auth/login");
        }
    }
}
