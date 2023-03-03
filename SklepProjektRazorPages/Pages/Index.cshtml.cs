using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace SklepProjektRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<Produkt>ProductList { get; set; } 
        
        public bool ProductListNotFound { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection()) {
                    ProductList = (List<Produkt>)conn.Query<Produkt>($"SELECT TOP 5 * FROM Produkt WHERE usuniety = 0 ORDER BY NEWID()");
                }

            }
            catch (InvalidOperationException exc)
            {
                ProductListNotFound = true;
            }
        }
    }
}