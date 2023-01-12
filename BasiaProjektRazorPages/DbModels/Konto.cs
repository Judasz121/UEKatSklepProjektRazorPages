namespace BasiaProjektRazorPages.DbModels
{
    public class Konto
    {
        public int ID_Konta { get; set; }
        public int ID_Klienta { get; set; }
        public int ID_Dostawcy { get; set; }
        public string LoginUzytkownika { get; set; }
        public string HashHasla { get; set; }
        public Int16 JestAdminem { get; set; }
    }
}
