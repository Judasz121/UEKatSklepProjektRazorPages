namespace BasiaProjektRazorPages.DBModels
{
    public class Produkt_magazyn
    {
        int ID_Produkt_magazyn { get; set; }
        int ID_Magazynu { get; set; }    

        int ID_Produktu { get; set; }

        int Ilosc_produktu { get; set; }
    }
}
