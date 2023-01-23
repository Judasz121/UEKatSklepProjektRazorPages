namespace BasiaProjektRazorPages.DbModels
{
    public class Produkt_magazyn_dostawa : BaseDbModel
    {
        public int? ID_Produkt_magazyn_dostawa { get; set; }
        public int? ID_Produkt_magazyn { get; set; }
        public int? ID_Dostawy { get; set; }

    }
}
