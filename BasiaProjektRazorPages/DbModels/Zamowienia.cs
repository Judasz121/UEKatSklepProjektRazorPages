namespace BasiaProjektRazorPages.DbModels
{
    public class Zamowienia
    {
        int ID_Zamowienia { get; set; } 
        int ID_Koszyka { get; set; }

        int ID_Klienta { get; set; }

        int ID_Adresu { get; set; }

        DateTime Data_zamowienia { get; set; }
        //Sprawdzic czy bool jest ok (bit w sql)
        bool Zaplacone { get; set; }
    }
}
