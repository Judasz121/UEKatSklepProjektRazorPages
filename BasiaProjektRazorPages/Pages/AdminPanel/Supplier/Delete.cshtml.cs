using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Supplier
{
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string redirect { get; set; }
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public IActionResult OnGet()
        {





            if (!string.IsNullOrWhiteSpace(redirect))
                return RedirectToPage(redirect);
            else
                return Page();
        }
    }
}
