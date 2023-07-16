using System.Security.Claims;
using client.Pages.Config;
using Client.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using server.Dto.Auth;

namespace Client.Pages.Auth
{
    public class LoginModel : PageModel
    {

        private readonly ServerConfig _serverConfig;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly ApiHelper _apiHelper = new ApiHelper(_httpClient);

        [BindProperty]
        public LoginRequest? LoginRequest { get; set; }

        public LoginModel(ServerConfig serverConfig)
        {
            _serverConfig = serverConfig;
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
    }
}
