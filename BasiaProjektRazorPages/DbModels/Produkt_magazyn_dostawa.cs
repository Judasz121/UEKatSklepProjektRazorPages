namespace BasiaProjektRazorPages.DbModels
{
    public class Produkt_magazyn_dostawa
    {
        int ID_Produkt_magazyn_dostawa { get; set; }
        int ID_Produkt_magazyn { get; set; }
        int ID_Dostawy { get; set; }
        int Ilosc_produktu { get; set; }

    }
}
