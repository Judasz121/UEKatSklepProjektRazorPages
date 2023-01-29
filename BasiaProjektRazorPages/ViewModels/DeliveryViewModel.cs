using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Dapper;


using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.ViewModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Dapper;

namespace BasiaProjektRazorPages.ViewModels
{
    public class DeliveryViewModel : Dostawa
    {
        public Dostawca Supplier { get; set; }
        public List<ProductsDeliveredToWarehouseViewModel> ProductsDeliveredToWarehouses { get; set; } = new List<ProductsDeliveredToWarehouseViewModel>();

        public DeliveryViewModel(Dostawa delivery, bool fetchAssociations = false)
        {
            this.ID_Dostawy = delivery.ID_Dostawy;
            this.ID_Dostawcy = delivery.ID_Dostawcy;
            this.Data_zrealizowania = delivery.Data_zrealizowania;
            this.Data_zamowienia = delivery.Data_zamowienia;

            if (fetchAssociations)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    try
                    {
                        this.Supplier = conn.QueryFirst<Dostawca>("SELECT TOP 1 * FROM Dostawca WHERE ID_Dostawcy = @ID_Dostawcy", delivery);                        
                    }
                    catch (InvalidOperationException exc) { }
                }
                this.FetchProductsDeliveredToWarehouses();
            }
        }
        public DeliveryViewModel(Dostawa delivery, Dostawca supplier, bool fetchAssociations = false) : this(delivery, false)
        {
            this.Supplier = supplier;

            if (fetchAssociations)
                this.FetchProductsDeliveredToWarehouses();
        }

        public void FetchProductsDeliveredToWarehouses()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                var pmds = conn.Query<Produkt_magazyn_dostawa>("SELECT * FROM Produkt_magazyn_dostawa WHERE ID_Dostawy = @ID_Dostawy", this);
                var alreadyHandledWarehousesId = new List<int>();
                Dostawa thisDelivery = new Dostawa()
                {
                    ID_Dostawy = this.ID_Dostawy,
                    ID_Dostawcy = this.ID_Dostawcy,
                    Data_zrealizowania = this.Data_zrealizowania,
                    Data_zamowienia = this.Data_zamowienia
                };
                foreach (Produkt_magazyn_dostawa pmd in pmds)
                {
                    if (pmd.ID_Magazynu == null || alreadyHandledWarehousesId.Contains((int)pmd.ID_Magazynu))
                        continue;
                    this.ProductsDeliveredToWarehouses.Add(new ProductsDeliveredToWarehouseViewModel(thisDelivery, (int)pmd.ID_Magazynu));
                    alreadyHandledWarehousesId.Add((int)pmd.ID_Magazynu);
                }
            }
        }
    }
}
