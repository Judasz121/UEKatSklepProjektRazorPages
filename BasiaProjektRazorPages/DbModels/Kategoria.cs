namespace BasiaProjektRazorPages.DbModels
{
    public class Kategoria : BaseDbModel
    {
        public int ID_Kategorii { get; set; }

        public int ID_Produktu { get; set; }

        public string Nazwa { get; set; }

        public int ID_Rodzica { get; set; } 

        //Sprawdzic czy ma byc bool czy int (sql sms ma bit ustawiony)
        public bool Jest_cyfrowy { get; set; }
    }
}
