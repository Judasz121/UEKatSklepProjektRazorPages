namespace BasiaProjektRazorPages.DBModels
{
    public class Koszyk
    {
        public int ID_Zamowienia { get; set; }

        public int ID_Produktu { get; set; }

        public int Ilosc_produktow { get; set; }

        public int ID_Koszyka { get; set; }
    }
}
