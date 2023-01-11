namespace BasiaProjektRazorPages.Models.DBModels
{
    public class Kategoria
    {
        int ID_Kategorii { get; set; }

        int ID_Produktu { get; set; }

        string Nazwa { get; set; }

        int ID_Rodzica { get; set; } 

        //Sprawdzic czy ma byc bool czy int (sql sms ma bit ustawiony)
        bool Jest_cyfrowy { get; set; }
    }
}
