namespace BasiaProjektRazorPages.DbModels
{
    public class Produkt_magazyn : BaseDbModel
    {
        public int ID_Produkt_magazyn { get; set; }
        public int ID_Magazynu { get; set; }    

        public int ID_Produktu { get; set; }

        public int Ilosc_produktu { get; set; }
    }
}
