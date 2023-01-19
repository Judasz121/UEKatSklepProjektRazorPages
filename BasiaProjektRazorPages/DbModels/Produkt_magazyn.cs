namespace BasiaProjektRazorPages.DbModels
{
    public class Produkt_magazyn : BaseDbModel
    {
        int ID_Produkt_magazyn { get; set; }
        int ID_Magazynu { get; set; }    

        int ID_Produktu { get; set; }

        int Ilosc_produktu { get; set; }
    }
}
