using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using System.Data;
using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.DbModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SklepProjektRazorPages.Pages.AdminPanel.Product
{
    public class AddModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public int id { get; set; }

        [BindProperty]
        public Produkt product { get; set; }

        public List<SelectListItem> categories { get; set; }  = new List<SelectListItem>();
        [BindProperty,DataType(DataType.Upload)]
        public IFormFile productCoverPhoto { get;set; }
        public string accountAlertClass { get; set; }
        public string accountAlertValue { get; set; }

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

        public IActionResult OnPost()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                var verification = Produkt.VerifyValues(product.Nazwa, product.Cena_jednostkowa);
                if (verification.Item1)
                {
                    conn.Execute($"INSERT INTO Produkt VALUES(@Nazwa,@Cena_jednostkowa,Null,@sciezkaZdjecia,0)", product);
                    product.sciezkaZdjecia = this.MoveToDbImageStorageFolder(productCoverPhoto).Split("wwwroot")[1];
                    product.sciezkaZdjecia = DbHelper.ReplacePolishChars(product.sciezkaZdjecia);
                    accountAlertClass = "alert-success";
                    accountAlertValue = "Pomyœlnie dodano";
                }
                else
                {
                    accountAlertClass = "alert-danger";
                    accountAlertValue = verification.Item2;
                }
            } 

            return Page();
        }
    }
}
