using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Dapper;


namespace BasiaProjektRazorPages.ViewModels
{
    public class DeliveryViewModel : Dostawa
    {
        public Dostawca Supplier { get; set; }

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
            }
        }
        public DeliveryViewModel(Dostawa delivery, Dostawca supplier) : this(delivery, false)
        {
            this.Supplier = supplier;
        }
    }
}
