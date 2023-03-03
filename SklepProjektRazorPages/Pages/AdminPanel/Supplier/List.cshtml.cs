using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;
using Dapper;

namespace SklepProjektRazorPages.Pages.AdminPanel.Supplier
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
