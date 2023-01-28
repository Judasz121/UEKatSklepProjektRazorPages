namespace BasiaProjektRazorPages.DbModels
{
    public class Produkt_magazyn_dostawa : BaseDbModel
    {
        public int? ID_Produkt_magazyn_dostawa { get; set; }
        public int? ID_Magazynu { get; set; }
        public int? ID_Dostawy { get; set; }
        public int? Ilosc_produktu { get; set; }
        public int? ID_Produktu { get; set; }

    }
}
