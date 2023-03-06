using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;
using Dapper;


namespace SklepProjektRazorPages.ViewModels
{
    public class OrderViewModel : Zamowienie
    {
        public List<CartRecord> cart { get; set; } = new List<CartRecord>();
        public Adres address { get; set; }

        public OrderViewModel(Zamowienie zam, bool fetchAssociations = true)
        {
            this.ID_Zamowienia = zam.ID_Zamowienia;
            this.Zaplacone = zam.Zaplacone;
            this.Data_zamowienia = zam.Data_zamowienia;
            this.Zlozone = zam.Zlozone;
            

            if (fetchAssociations)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    // products
                    IEnumerable<Koszyk> cart = conn.Query<Koszyk>("SELECT * FROM Koszyk WHERE ID_Zamowienia = @ID_Zamowienia", zam);
                    foreach(Koszyk c in cart)
                    {
                        Produkt p = conn.QueryFirst<Produkt>("SELECT * FROM PRODUKT WHERE ID_Produktu = @ID_Produktu", c);
                        this.cart.Add(new CartRecord
                        {
                            cartId = (int)c.ID_Koszyka,
                            amount = (int)c.Ilosc_produktow,
                            product = p
                        });
                    }

                    // address
                    try
                    {
                        this.address = conn.QueryFirst<Adres>("SELECT * FROM Adres WHERE ID_Adresu = @ID_Adresu", zam);
                    }
                    catch(InvalidOperationException exc) { }
                }
            }
        }
    }
    public struct CartRecord
    {
        public Produkt product { get; set; }
        public int amount { get; set; }
        public int cartId { get; set; }
    }
}