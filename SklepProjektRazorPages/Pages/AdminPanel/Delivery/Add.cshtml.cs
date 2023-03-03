using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.ViewModels;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SklepProjektRazorPages.Pages.AdminPanel.Delivery
{
    public class AddModel : PageModel
    {
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string supplierId { get; set; }
        public List<SelectListItem> Suppliers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Warehouses { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public Dostawa Delivery { get; set; }
        [BindProperty]
        public List<Produkt_magazyn_dostawa> Products_warehouses_delivery { get; set; }
        public void OnGet()
        {
            this.FillAllSelectListItems();
        }

        public void OnPost()
        {
            this.FillAllSelectListItems();

            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                int? deliveryId = null;
                bool ok = true;
                // delivery
                try
                {
                    if (Delivery.Data_zrealizowania == DateTime.MinValue)
                        Delivery.Data_zamowienia = null;
                    if (Delivery.Data_zrealizowania == DateTime.MinValue)
                        Delivery.Data_zrealizowania = null;
                    deliveryId = conn.ExecuteScalar<int>("INSERT INTO Dostawa (ID_Dostawcy, Data_zamowienia, Data_zrealizowania)" +
                        "Values (@ID_Dostawcy, @Data_zamowienia, @Data_zrealizowania); SELECT SCOPE_IDENTITY();",
                        this.Delivery);
                }
                catch(InvalidOperationException exc)
                {
                    ok = false;
                    alertClass = "alert-danger";
                    alertMessage = "Server Error:\n" + exc.Message;
                }

                // product-warehouse-delivery
                if (ok)
                    try
                    {
                        foreach (Produkt_magazyn_dostawa pmd in this.Products_warehouses_delivery)
                        {
                            pmd.ID_Dostawy = deliveryId;
                            conn.Execute("INSERT INTO Produkt_magazyn_dostawa (ID_Magazynu, ID_Dostawy, Ilosc_produktu, ID_Produktu)" +
                                "VALUES(@ID_Magazynu, @ID_Dostawy, @Ilosc_produktu, @ID_Produktu)", pmd);
                        }
                    }
                    catch (InvalidOperationException exc)
                    {
                        ok = false;
                        alertClass = "alert-danger";
                        alertMessage = "Server Error:\n" + exc.Message;
                    }
                if (ok)
                {
                    alertClass = "alert-success";
                    alertMessage = "Pomyœlnie dodano";
                }
            }
        }



        #region selectListItems
        public void FillAllSelectListItems()
        {
            this.FillSupplierSelectListItems();
            this.FillWarehouseSelectListItems();
            this.FillProductSelectListItems();
        }

        public void FillSupplierSelectListItems()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbSuppliers = conn.Query<Dostawca>("SELECT * FROM Dostawca");
                    foreach (Dostawca sup in dbSuppliers)
                    {
                        this.Suppliers.Add(new SelectListItem { Text = sup.Nazwa, Value = sup.ID_Dostawcy.ToString() });
                    }
                }
                catch (InvalidOperationException exc) { }
            }
        }
        public void FillProductSelectListItems()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbProducts = conn.Query<Produkt>("SELECT * FROM Produkt");
                    foreach (Produkt prod in dbProducts)
                    {
                        this.Products.Add(new SelectListItem { Text = prod.Nazwa, Value = prod.ID_Produktu.ToString() });
                    }
                }
                catch (InvalidOperationException exc) { }
            }
        }
        public void FillWarehouseSelectListItems()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbWarehouses = conn.Query<Magazyn>("SELECT * FROM Magazyn");
                    foreach (Magazyn war in dbWarehouses)
                    {
                        this.Warehouses.Add(new SelectListItem { Text = war.Nazwa, Value = war.ID_Magazynu.ToString() });
                    }
                }
                catch (InvalidOperationException exc) { }
            }
        }
        #endregion selectListItems
    }
}
