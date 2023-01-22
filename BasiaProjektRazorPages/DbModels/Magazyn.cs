namespace BasiaProjektRazorPages.DbModels
{
    public class Magazyn : BaseDbModel
    {
        public int ID_Magazynu { get; set; }

        public string Nazwa { get; set; }  

        public string Kraj { get; set; }

        public string Miasto { get; set; } 

        public string Kod_pocztowy { get; set; }  

        public string Ulica { get; set; }
        
        public string Numer_budynku { get; set; }
       
    }
}
