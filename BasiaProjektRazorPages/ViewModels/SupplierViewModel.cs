using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using System.Data;
using Dapper;

namespace BasiaProjektRazorPages.ViewModels
{
    public class SupplierViewModel : Dostawca
    {
        public List<Dostawa> Deliveries { get; set; } = new List<Dostawa>();
        
        public SupplierViewModel(Dostawca dos, bool fetchAssociations = false)
        {
            this.ID_Dostawcy = dos.ID_Dostawcy;
            this.Nazwa = dos.Nazwa;
            this.Numer_telefonu = dos.Numer_telefonu;
            this.Email = dos.Email;
            this.NIP = dos.NIP;

            if (fetchAssociations)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    try
                    {
                        this.Deliveries = conn.Query<Dostawa>("SElECT * FROM Dostawa WHERE ID_Dostawcy = @ID_Dostawcy", dos).ToList();                        
                    }
                    catch(InvalidOperationException exc)
                    {

                    }
                }
            }
        }

    }
}
