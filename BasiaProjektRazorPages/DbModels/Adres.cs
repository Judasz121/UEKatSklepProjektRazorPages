namespace BasiaProjektRazorPages.DbModels
{
    public class Adres
    {
        public int ID_Adresu { get; set; }
        public int ID_Klienta { get; set; }
        public string Kraj { get; set; }
        public string Miasto { get; set; }
        public string  Ulica { get; set; }
        public string Kod_pocztowy { get; set; }
        public  string Numer_budynku { get; set; }
        public string Numer_mieszkania { get; set; }

    }
}
