using client.Config.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Settings;

[FptAuthorize]
public class IndexModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        if (TempData["Error"] is string errorMessage)
        {
            ViewData["Error"] = errorMessage;
        }
        return Page();
    }
}