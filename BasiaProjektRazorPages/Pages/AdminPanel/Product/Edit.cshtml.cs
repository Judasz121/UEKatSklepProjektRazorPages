using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Product
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        [BindProperty]
        public Produkt product { get; set; }
        public List<SelectListItem> categories { get; set; } = new List<SelectListItem>();
        [BindProperty, DataType(DataType.Upload)]
        public IFormFile productCoverPhoto { get; set; }

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
            this.FetchCategoriesSelectItems();
        }
        public IActionResult OnPost()
        {
            this.FetchCategoriesSelectItems();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(product.Nazwa))
            {
                ok = false;
                alertMessage = "Nazwa nie mo¿ê byæ pusta";
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
                    if (this.productCoverPhoto != null && productCoverPhoto.Length != 0)
                    {
                        product.sciezkaZdjecia = this.MoveToDbImageStorageFolder(productCoverPhoto).Split("wwwroot")[1];
                        product.sciezkaZdjecia = DbHelper.ReplacePolishChars(product.sciezkaZdjecia);
                    }

                    else
                    {
                        Produkt oldProduct = conn.QueryFirst<Produkt>($"SELECT * FROM Produkt WHERE ID_Produktu = '{product.ID_Produktu}'");
                        product.sciezkaZdjecia = oldProduct.sciezkaZdjecia;
                    }
                    try
                    {
                        //string sql = $"UPDATE Produkt SET Nazwa = '{product.Nazwa}', Cena_jednostkowa = '{product.Cena_jednostkowa}', ID_Kategorii = '{product.ID_Kategorii}', sciezkaZdjecia = '{product.sciezkaZdjecia}' WHERE ID_Produktu = '{product.ID_Produktu}'";
                        string sql = $"UPDATE Produkt SET Nazwa = @Nazwa, Cena_jednostkowa = @Cena_jednostkowa, ID_Kategorii = @ID_Kategorii, sciezkaZdjecia = @sciezkaZdjecia WHERE ID_Produktu = @ID_Produktu ";

                        conn.Execute(sql, product);
                        alertClass = "alert-success";
                        alertMessage = "Zapisano";
                    }
                    catch (InvalidOperationException exc)
                    {
                        alertClass = "alert-danger";
                        alertMessage = "Server error: \n" + exc.Message;
                    }
                }
            }
            return Page();
        }

        public string MoveToDbImageStorageFolder(IFormFile file)
        {
            string dirPath = DbHelper.absoluteImageStorageFolderPath;
            string finalRelativeFilePath = null;

            bool nameTaken = true;
            int i = 0;
            while (nameTaken)
            {
                string newFileName = file.FileName.Split('.')[0] + "_" + i.ToString() + '.' + file.FileName.Split('.')[1];
                newFileName = DbHelper.ReplacePolishChars(newFileName);
                string filePath = Path.Combine(dirPath, newFileName);
                if (!System.IO.File.Exists(filePath))
                {
                    nameTaken = false;
                    finalRelativeFilePath = Path.Combine(DbHelper.relativeImageStorageFolderPath, newFileName);                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                else
                {
                    i++;
                }
            }
            return finalRelativeFilePath;
        }

        public void FetchCategoriesSelectItems()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    var dbCats = conn.Query<Kategoria>("SELECT * FROM Kategoria");
                    foreach (Kategoria cat in dbCats)
                    {
                        this.categories.Add(new SelectListItem()
                        {
                            Value = cat.ID_Kategorii.ToString(),
                            Text = cat.Nazwa
                        });
                    }
                }
            }
            catch (InvalidOperationException exc)
            {
                alertClass = "alert-danger";
                alertMessage += "Server Error: \n" + exc.Message;
            }
        }
    }
}
