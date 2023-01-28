using System.Reflection;
using System.Text.RegularExpressions;
namespace BasiaProjektRazorPages.DbModels
{
    public class Klient : BaseDbModel
    {
        public int? ID_Klienta { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Telefon { get; set; }

        public static Tuple<bool,string> VerifyValues(string name, string surname, string phone)
        {
            bool ok = true;
            string msg = "";
            Regex nonDigit = new Regex(@"/D");
            Regex nonLetter = new Regex(@"[^a-zA-Z]");
            if (!string.IsNullOrWhiteSpace(phone) && nonDigit.IsMatch(phone))
            {
                ok = false;
                msg = "Nr telefonu jest nieprawidłowy";
            }
            if (!string.IsNullOrWhiteSpace(name) && nonLetter.IsMatch(name))
            {
                if (!ok)
                    msg += "\n";
                ok = false;
                msg += "Imię zawiera znaki niedozwolone";
            }
            if (!string.IsNullOrWhiteSpace(surname) && nonLetter.IsMatch(surname))
            {
                if (!ok)
                    msg += "\n";
                ok = false;
                msg += "Nazwisko zawiera znaki niedozwolone";
            }            
            return new Tuple<bool, string>(ok, msg);
        }
        public Tuple<bool, string> VerifyInstanceValues(bool ignoreNullProps = false)
        {
            if (ignoreNullProps)
                return Klient.VerifyValues(this.Imie, this.Nazwisko, this.Telefon);
            else
            {
                Klient temp = (Klient)this.MemberwiseClone();
                return Klient.VerifyValues(temp.Imie, temp.Nazwisko, temp.Telefon);
            }
        }

    }
}
