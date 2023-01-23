namespace BasiaProjektRazorPages.DbModels
{
    public class Dostawa
    {
        public int ID_Dostawy { get; set; }

        public int ID_Dostawcy { get; set; }

        public int ID_Magazynu { get; set; }

        public int ID_Produkt_magazyn { get; set; }

        public int Ilosc_produktu { get; set; }
    }
}
