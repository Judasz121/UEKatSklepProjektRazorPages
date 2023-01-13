using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using Dapper;

namespace BasiaProjektRazorPages.Pages.AdminPanel
{
    public class AccountsModel : PageModel
    {
        public IEnumerable<Konto> accounts{ get; set; }

        public void OnGet()
        {            
               
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
