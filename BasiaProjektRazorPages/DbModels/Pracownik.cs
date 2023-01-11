namespace BasiaProjektRazorPages.Models.DBModels
{
    public class Pracownik
    {
        int ID_Pracownika { get; set; }

        string Imie { get; set; }    

        string Nazwisko { get; set; }

        int ID_Magazynu { get; set; }

        int Wyplata { get; set; }

        string Numer_telefonu { get; set; }

        string PESEL { get; set; }

        string Numer_konta { get; set; }

        DateTime Data_Zatrudnienia { get; set; }   

        DateTime Data_Zwolnienia { get; set; }
    }
}
