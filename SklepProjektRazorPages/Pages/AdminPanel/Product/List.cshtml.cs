using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using System.Data;
using SklepProjektRazorPages.Helpers;
using Dapper;

namespace SklepProjektRazorPages.Pages.AdminPanel.Product
{
    public class ListModel : PageModel
    {
        public IEnumerable<Produkt> products { get; set; }

        public List<Kategoria> CategoryNames { get; set; }  = new List<Kategoria>();

        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                products = conn.Query<Produkt>("SELECT * FROM Produkt");
                CategoryNames = (List<Kategoria>)conn.Query <Kategoria>("SELECT * FROM Kategoria");
            }
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
