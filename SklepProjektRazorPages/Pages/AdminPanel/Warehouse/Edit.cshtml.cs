using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using SklepProjektRazorPages.ViewModels;

namespace SklepProjektRazorPages.Pages.AdminPanel.Warehouse
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty]
        public Magazyn warehouse { get; set; }
        public List<SelectListItem> allProducts { get; set; }
        [BindProperty]
        public List<Produkt_magazyn> newProducts { get; set; }

        public List<ProductViewModel> warehouseProducts { get; set; }
        public List<Produkt_magazyn> product_warehouse { get; set; }
        public string alertClass { get; set; }
        public string alertValue { get; set; }
        public bool warehouseNotFound { get; set; }
        public void OnGet()
        {            
            this.FillProductsSelectListItem();

            // get warehouse
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    warehouse = conn.QueryFirst<Magazyn>($"SELECT TOP 1 * FROM Magazyn WHERE ID_Magazynu = '{id}'");
                }
            }
            catch (InvalidOperationException ex)
            {
                warehouseNotFound = true;
            }

            // get warehouse products
            try
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    this.product_warehouse = conn.Query<Produkt_magazyn>("SELECT * FROM Produkt_magazyn WHERE ID_Magazynu = @ID_Magazynu ", this.warehouse).ToList();
                    if (this.product_warehouse.Count == 0)
                        this.warehouseProducts = new List<ProductViewModel>();
                    else
                        foreach (Produkt_magazyn p_m in product_warehouse)
                        {
                            this.warehouseProducts.Add(new ProductViewModel(p_m.ID_Produktu, true));
                        }
                }
            }
            catch (InvalidOperationException exc)
            {
                alertClass = "alert-danger";
                if (!string.IsNullOrWhiteSpace(this.alertValue))
                    alertValue += "\n Server Error: " + exc.Message;
            }
        }
        public IActionResult OnPost()
        {
            if (!AccountHelper.loggedInVerified)
                return RedirectToPage("/Account/Login");
            #region warehouse props update
            Magazyn oldWarehouse = null;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                oldWarehouse = conn.QueryFirst<Magazyn>($"SELECT TOP 1 * FROM Magazyn WHERE ID_Magazynu = '{warehouse.ID_Magazynu}'");
            }
            if(!warehouse.Equals(oldWarehouse)) {
                var verification = Magazyn.verifyValues(warehouse.Nazwa, warehouse.Kraj, warehouse.Miasto, warehouse.Kod_pocztowy, warehouse.Ulica, warehouse.Numer_budynku);
                if (verification.Item1)
                {
                    using(IDbConnection conn = DbHelper.GetDbConnection())
                    {
                        conn.Execute($"UPDATE Magazyn SET Nazwa = '{warehouse.Nazwa}', Kraj = '{warehouse.Kraj}', Miasto = '{warehouse.Miasto}', Kod_pocztowy = '{warehouse.Kod_pocztowy}', Ulica = '{warehouse.Ulica}', Numer_budynku = '{warehouse.Numer_budynku}' WHERE ID_Magazynu = '{warehouse.ID_Magazynu}'");
                    }
                    alertClass = "alert-success";
                    alertValue = "Zaktualizowano";
                }
                else
                {
                    alertClass = "alter-danger";
                    alertValue = verification.Item2;
                }
                
            }
            #endregion warehouse props update

            return Page();
        }

        public void FillProductsSelectListItem()
        {
            try
            {
                this.allProducts = this.allProducts == null ? new List<SelectListItem>() : this.allProducts;
                IDbConnection conn = DbHelper.GetDbConnection();
                IEnumerable<Produkt> dbProducts = conn.Query<Produkt>("SELECT * FROM Produkt");
                conn.Dispose();
                foreach(Produkt p in dbProducts)
                {
                    this.allProducts.Add(new SelectListItem(p.Nazwa, p.ID_Produktu.ToString()));
                }

            }
            catch(InvalidOperationException exc)
            {
                if (!string.IsNullOrEmpty(alertValue))
                {
                    alertValue += "\n Server Error: " + exc.Message;
                    alertClass = "alert-danger";
                }
            }
        }
    }
}
