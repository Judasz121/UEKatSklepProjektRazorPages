using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;
using Dapper;

namespace SklepProjektRazorPages.Pages.AdminPanel.Supplier
{
    public class AddModel : PageModel
    {
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        [BindProperty]
        public Dostawca supplier { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            bool success = false;
            var verification = supplier.VerifyInstanceValues();
            if (verification.Item1)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    try
                    {
                        conn.Execute("INSERT INTO Dostawca (Nazwa, Numer_telefonu, Email, NIP) VALUES(@Nazwa, @Numer_telefonu, @Email, @NIP)", this.supplier);
                        success = true;
                        alertClass = "alert-success";
                        alertMessage = "Dodano pomy�lnie";
                    }
                    catch (InvalidOperationException exc)
                    {
                        success = false;
                        alertMessage = "Server Error:\n" + exc.Message;
                        alertClass = "alert-danger";
                    }
                }
            }
            else
            {
                alertClass = "alert-warning";
                alertMessage = verification.Item2;
            }

            return Page();
        }
    }
}
