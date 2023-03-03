using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;
using Dapper;

namespace SklepProjektRazorPages.ViewModels
{
    public class ProductsDeliveredToWarehouseViewModel
    {
        public Dostawa Delivery { get; set; }
        public Magazyn Warehouse { get; set; }
        public List<Tuple<Produkt, int>> Products { get; set; } = new List<Tuple<Produkt, int>>();




        public ProductsDeliveredToWarehouseViewModel(Dostawa delivery, Magazyn warehouse)
        {
            this.Delivery = delivery;
            this.Warehouse = warehouse;
            this.FetchProducts();
        }
        public ProductsDeliveredToWarehouseViewModel(Dostawa delivery, int warehouseId)
        {
            this.Delivery = delivery;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                this.Warehouse = conn.QueryFirst<Magazyn>("SElECT * FROM Magazyn WHERE ID_Magazynu = @id", new { id = warehouseId });
            }
            this.FetchProducts();
        }
        public ProductsDeliveredToWarehouseViewModel(int deliveryId, int warehouseId)
        {           
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                this.Delivery = conn.QueryFirst<Dostawa>("SElECT * FROM Dostawa WHERE ID_Dostawy = @id", new { id = deliveryId });
                this.Warehouse = conn.QueryFirst<Magazyn>("SElECT * FROM Magazyn WHERE ID_Magazynu = @id", new { id = warehouseId });
            }
            this.FetchProducts();
        }
        public void FetchProducts()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {                    
                    var pmds = conn.Query<Produkt_magazyn_dostawa>("SElECT * FROM Produkt_magazyn_dostawa WHERE ID_Dostawy = @ID_Dostawy AND ID_Magazynu = @ID_Magazynu",
                        new { ID_Magazynu = this.Warehouse.ID_Magazynu, ID_Dostawy = this.Delivery.ID_Dostawy });
                    foreach (Produkt_magazyn_dostawa pmd in pmds)
                    {
                        Produkt prod = conn.QueryFirst<Produkt>("SELECT * FROM Produkt WHERE ID_Produktu = @ID_Produktu", pmd);
                        Products.Add(new Tuple<Produkt, int>(prod, (int)pmd.Ilosc_produktu));
                    }
                }
                catch (InvalidOperationException exc) { }
            }
        }

    }
}
