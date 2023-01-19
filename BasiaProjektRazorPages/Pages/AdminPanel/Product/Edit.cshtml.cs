using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Product
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        [BindProperty]
        public Produkt product { get; set; }
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public bool productNotFound { get; set; }
        public void OnGet()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    product = conn.QueryFirst<Produkt>($"SELECT TOP 1 * FROM Produkt WHERE ID_Produktu = '{id}'");
                }
            }
            catch (InvalidOperationException exc)
            {
                productNotFound = true;
            }
        }
        public IActionResult OnPost()
        {
            bool ok = true;
            if (string.IsNullOrWhiteSpace(product.Nazwa))
            {
                ok = false;
                alertMessage = "Nazwa nie mo¿ê byæ pust¹ suk¹";
            }
            if (product.Cena_jednostkowa == null)
            {
                ok = false;
                if (!string.IsNullOrWhiteSpace(alertMessage))
                    alertMessage += "\n";
                alertMessage += "Cena nie mo¿e byæ pusta";
            }
            var verification = Produkt.VerifyValues(product.Nazwa, product.Cena_jednostkowa);
            if (!ok || !verification.Item1)
            {
                alertClass = "alert-danger";
                if (string.IsNullOrEmpty(alertMessage) == false)
                    alertMessage += "\n";
                if (!verification.Item1)
                    alertMessage += verification.Item2;
            }
            else
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    try
                    {
                        conn.Execute($"UPDATE Produkt SET Nazwa = '{product.Nazwa}', Cena_jednostkowa = '{product.Cena_jednostkowa}' WHERE ID_Produktu = '{product.ID_Produktu}'");
                        alertClass = "alert-success";
                        alertMessage = "Zapisano";
                    }
                    catch (Exception exc)
                    {
                        alertClass = "alert-danger";
                        alertMessage = "Server error: \n" + exc.Message;
                    }
                }
            }
            return Page();
        }
    }
}
