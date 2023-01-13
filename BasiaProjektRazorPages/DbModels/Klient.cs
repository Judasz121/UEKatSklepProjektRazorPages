using System.Text.RegularExpressions;
namespace BasiaProjektRazorPages.DbModels
{
    public class Klient
    {
        public int ID_Klienta { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Telefon { get; set; }

        public static Tuple<bool,string> VerifyValues(string name, string surname, string phone)
        {
            bool ok = true;
            string msg = "";
            Regex nonDigit = new Regex(@"/D");
            Regex nonLetter = new Regex(@"[^a-zA-Z]");
            if (nonDigit.IsMatch(phone))
            {
                ok = false;
                msg = "Nr telefonu jest nieprawidłowy";
            }
            if (nonLetter.IsMatch(name))
            {
                if (!ok)
                    msg += "\n";
                ok = false;
                msg += "Imię zawiera znaki niedozwolone";
            }
            if (nonLetter.IsMatch(surname))
            {
                if (!ok)
                    msg += "\n";
                ok = false;
                msg += "Nazwisko zawiera znaki niedozwolone";
            }            
            return new Tuple<bool, string>(ok, msg);
        }
        
    }
}
