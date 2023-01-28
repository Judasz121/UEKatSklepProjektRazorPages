using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Dapper;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Supplier
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        public bool supplierNotFound { get; set; } = false;
        public string alertMessage { get; set; }
        public string alertClass { get; set; }
        [BindProperty]
        public Dostawca supplier { get; set; }

        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    supplier = conn.QueryFirst<Dostawca>("SElECT TOP 1 * FROM Dostawca WHERE ID_Dostawcy = @id", new { id = this.id });
                }
                catch (InvalidOperationException exc)
                {
                    supplierNotFound = true;                 
                }
            }
        }

        public void OnPost()
        {
            var verification = supplier.VerifyInstanceValues();
            if (verification.Item1)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    try
                    {
                        conn.Execute("UPDATE Dostawca SET Nazwa = @Nazwa, Numer_telefonu = @Numer_telefonu, Email = @Email, NIP = @NIP WHERE ID_Dostawcy = @ID_Dostawcy", this.supplier);
                        alertClass = "alert-success";
                        alertMessage = "Pomyœlnie zapisano";
                    }
                    catch (InvalidOperationException exc)
                    {
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

        }
    }
}
