using System.Text.RegularExpressions;

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

        public bool Usunieta { get; set; }  
        public static Tuple<bool, string> verifyValues(string? Nazwa, string? Kraj,string? Miasto, string? Kod_pocztowy, string? Ulica, string? Numer_budynku)
        {
            bool ok = true;
            string msg = "";
            Regex nonDigit = new Regex(@"/D");
            Regex nonLetter = new Regex(@"[^a-zA-Z]");
            if (Nazwa != null)
            {
                if (!string.IsNullOrEmpty(Nazwa) && nonLetter.IsMatch(Nazwa))
                {
                    ok = false;
                    msg += "Nazwa nie może być pusta.\n";
                }
                if (Nazwa.Length > 30)
                {
                    ok = false;
                    msg += "Taka nazwa nie istnieje.\n";
                }
            }

            if (Kraj != null)
            {
                if (!string.IsNullOrEmpty(Kraj) && nonLetter.IsMatch(Kraj))
                {
                    ok = false;
                    msg += "Kraj nie może być puste\n";
                }
                if (Kraj.Length > 20)
                {
                    ok = false;
                    msg += "Taki kraj nie istnieje\n";
                }
            }
            if (Miasto != null)
            {
                if (!string.IsNullOrEmpty(Miasto) && nonDigit.IsMatch(Miasto))
                {
                    ok = false;
                    msg += "Miasto nie może być puste\n";
                }
                if (Miasto.Length > 20)
                {
                    ok = false;
                    msg += "Takie miasto nie istnieje\n";
                }
            }
            if(Kod_pocztowy!= null)
            {
                if (string.IsNullOrEmpty(Kod_pocztowy))
                {
                    ok = false;
                    msg += "Kod pocztowy nie może być pusty\n";
                }
                if (Kod_pocztowy.Length > 10)
                {
                    ok = false;
                    msg += "Taki kod pocztowy nie istnieje.\n";
                }
            }

            if (Ulica != null && nonDigit.IsMatch(Ulica))
            {
                if (!string.IsNullOrEmpty(Ulica))
                {
                    ok = false;
                    msg += "Ulica nie może być pusty\n";
                }
                if (Ulica.Length > 20)
                {
                    ok = false;
                    msg += "Taka ulica nie istnieje.\n";
                }
            }

            if (Numer_budynku != null)
            {
                if (string.IsNullOrEmpty(Numer_budynku))
                {
                    ok = false;
                    msg += "Numer budynku nie może być pusty.\n";
                }
                if (Numer_budynku.Length > 3)
                {
                    ok = false;
                    msg += "Taki numer budynku nie istnieje.\n";
                }
            }
            return new Tuple<bool, string>(ok, msg);
        }
    }
}
