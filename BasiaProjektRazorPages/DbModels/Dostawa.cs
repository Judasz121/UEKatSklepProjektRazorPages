using System.ComponentModel;

namespace BasiaProjektRazorPages.DbModels
{
    public class Dostawa
    {
        public int? ID_Dostawy { get; set; }

        [DisplayName("Dostawca")]
        public int? ID_Dostawcy { get; set; }

        [DisplayName("Data zamówienia")]
        public DateTime Data_zamowienia { get; set; }

        [DisplayName("Data zrealizowania")]
        public DateTime Data_zrealizowania { get; set; }
    }
}
