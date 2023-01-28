using System.ComponentModel;

namespace BasiaProjektRazorPages.DbModels
{
    public class Dostawa
    {
        public int ID_Dostawy { get; set; }

        public int ID_Dostawcy { get; set; }

        public int ID_Produkt_magazyn { get; set; }

        public DateTime Data_zamowienia { get; set; }
        public DateTime Data_zrealizowania { get; set; }
    }
}
