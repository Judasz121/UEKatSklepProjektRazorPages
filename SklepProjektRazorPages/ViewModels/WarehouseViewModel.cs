using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;
using Dapper;
using Microsoft.Win32.SafeHandles;

namespace SklepProjektRazorPages.ViewModels
{

    public class WarehouseViewModel : Magazyn
    {
        public List<Produkt>ProductList { get; set; } = new List<Produkt>();

        public List<Pracownik> EmployeesList { get; set; } = new List<Pracownik>();

        
        public WarehouseViewModel(Magazyn mag)
        {
            this.ID_Magazynu = mag.ID_Magazynu;
            this.Nazwa = mag.Nazwa;
            this.Kraj = mag.Kraj;
            this.Ulica = mag.Ulica;
            this.Miasto = mag.Miasto;
            this.Kod_pocztowy = mag.Kod_pocztowy;
            this.Numer_budynku = mag.Numer_budynku;

            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var tag_productslist = conn.Query<Produkt_magazyn>($"SELECT * FROM Produkt_magazyn WHERE ID_Magazynu = @ID_Magazynu",mag);
                    foreach(Produkt_magazyn tag_productlist in tag_productslist)
                    {
                        ProductList.Add(conn.QueryFirst<Produkt>("SELECT * FROM Produkt WHERE ID_Produktu = @ID_Produktu", tag_productlist));
                        
                    }
                }
                catch
                {

                }

                try
                {
                    var tag_employeeslist = conn.Query<Pracownik>($"SELECT * FROM Pracownik WHERE ID_Magazynu = @ID_Magazynu", mag);
                    foreach (Pracownik tag_employeelist in tag_employeeslist)
                    {
                        EmployeesList.Add(conn.QueryFirst<Pracownik>("SELECT * FROM Pracownik WHERE ID_Pracownika = @ID_Pracownika", tag_employeelist));
                    }
                }
                catch
                {
                    
                }
            }
        }
    }
}
