namespace BasiaProjektRazorPages.DBModels
{
    public class Magazyn
    {
        int ID_Magazynu { get; set; }

        string Nazwa { get; set; }  

        string Kraj { get; set; }

        string Miasto { get; set; } 

        string Kod_pocztowy { get; set; }  

        string Ulica { get; set; }
        
        string Numer_budynku { get; set; }
       
        string Numer_mieszkania { get; set; }
    }
}
