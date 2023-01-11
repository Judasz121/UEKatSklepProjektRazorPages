namespace BasiaProjektRazorPages.DBModels
{
    public class Dostawa
    {
        int ID_Dostawy { get; set; }

        int ID_Dostawcy { get; set; }

        int ID_Magazynu { get; set; }

        int ID_Produkt_magazyn { get; set; }

        int Ilosc_produktu { get; set; }
    }
}
