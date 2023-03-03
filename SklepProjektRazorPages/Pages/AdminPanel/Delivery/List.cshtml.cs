using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace SklepProjektRazorPages.Pages.AdminPanel.Delivery
{
    public class ListModel : PageModel
    {
        public List<DeliveryViewModel> delivery { get; set; } = new List<DeliveryViewModel>();
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                var dbDeliveries = conn.Query<Dostawa>("SELECT * FROM Dostawa");
                foreach(Dostawa delivery in dbDeliveries)
                {
                    this.delivery.Add(new DeliveryViewModel(delivery, true));
                }
            }
        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
