namespace BasiaProjektRazorPages.DbModels
{
    public class Pracownik : BaseDbModel
    {
        public int ID_Pracownika { get; set; }

        public string Imie { get; set; }    

        public string Nazwisko { get; set; }

        public int ID_Magazynu { get; set; }

        public int Wyplata { get; set; }

        public string Numer_telefonu { get; set; }

        public string PESEL { get; set; }

        public string Numer_konta { get; set; }

        public DateTime Data_Zatrudnienia { get; set; }   

        public DateTime Data_Zwolnienia { get; set; }
    }
}
