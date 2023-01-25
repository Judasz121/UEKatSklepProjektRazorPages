using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
        public static Tuple<bool, string> verifyValues(string? Imie, string? Nazwisko, int? ID_Magazynu,int? Wyplata, string? Numer_telefonu,string? PESEL, string? Numer_konta)
        {
            bool ok = true;
            string msg = "";
            Regex nonDigit = new Regex(@"/D");
            Regex nonLetter = new Regex(@"[^a-zA-Z]");
            if (Imie != null)
            {
                if (!string.IsNullOrEmpty(Imie) && nonLetter.IsMatch(Imie))
                {
                    ok = false;
                    msg += "Imie nie może być pusta";
                }
                if (Imie.Length > 20)
                {
                    ok = false;
                    msg += "Takie imie nie istnieje.";
                }
            }
  
            if (Nazwisko != null)
            {
                if (string.IsNullOrEmpty(Nazwisko) && nonLetter.IsMatch(Nazwisko))
                {
                    ok = false;
                    msg += "Nazwisko nie może być puste";
                }
                if (Nazwisko.Length > 20)
                {
                    ok = false;
                    msg += "Takie nazwisko nie istnieje";
                }
            }

            if (ID_Magazynu != null)
            {
                if (ID_Magazynu <= 0)
                {
                    ok = false;
                    msg += "Taki magazyn nie istnieje";
                }
            }

            if(PESEL != null) { 
                if(PESEL.Length != 9)
                {
                    ok = false;
                    msg += "Taki Pesel nie istnieje";
                }
            }

            if (Wyplata != null)
            {
                if (Wyplata <= 3100)
                {
                    ok = false;
                    msg += "Taka wypłata nie może zostać zaaplikowana.";
                }
            }
          
            if (Numer_telefonu != null)
            {
                if (!string.IsNullOrEmpty(Numer_telefonu) && nonDigit.IsMatch(Numer_telefonu))
                {
                    ok = false;
                    msg += "Numer telefonu nie może być pusty";
                }
                if (Numer_telefonu.Length > 9)
                {
                    ok = false;
                    msg += "Taki numer telefonu nie istnieje";
                }
            }
  
            if (!string.IsNullOrEmpty(Numer_konta) && nonDigit.IsMatch(Numer_konta))
            {
                ok = true;
            }
            else if (Numer_konta.Length > 26)
            {
                ok = false;
                msg += "Taki numer konta nie istnieje.";
            }

            return new Tuple<bool, string>(ok, msg);
        }
    }
}
