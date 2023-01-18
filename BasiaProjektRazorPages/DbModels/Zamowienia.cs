namespace BasiaProjektRazorPages.DbModels
{
    public class Zamowienia
    {
        public int ID_Zamowienia { get; set; } 
        public int ID_Koszyka { get; set; }

        public int ID_Klienta { get; set; }

        public int ID_Adresu { get; set; }

        public DateTime Data_zamowienia { get; set; }
        //Sprawdzic czy bool jest ok (bit w sql)
        public bool Zaplacone { get; set; }
    }
}
