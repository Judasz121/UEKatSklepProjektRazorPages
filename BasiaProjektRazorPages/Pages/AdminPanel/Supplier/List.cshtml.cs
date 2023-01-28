using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Dapper;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Supplier
{
    public class ListModel : PageModel
    {
        public List<Dostawca> Suppliers { get; set; } = new List<Dostawca>();
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    this.Suppliers = conn.Query<Dostawca>("SELECT * FROM Dostawca").ToList();
                }
                catch(InvalidOperationException exc)
                {
                    // no suppliers
                }
            }
        }
    }
}
