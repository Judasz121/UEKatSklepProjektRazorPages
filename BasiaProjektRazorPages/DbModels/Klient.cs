using System.Reflection;
using System.Text.RegularExpressions;
namespace BasiaProjektRazorPages.DbModels
{
    public class Klient : IEquatable<Klient>
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

        bool IEquatable<Klient>.Equals(Klient? other)
        {
            //if (other.ID_Klienta != this.ID_Klienta)
            //    return false;
            //if (other.Imie != this.Imie)
            //    return false;
            //if (other.Nazwisko != this.Nazwisko)
            //    return false;
            //if (other.Telefon != this.Telefon)
            //    return false;
            //return true;

            foreach(PropertyInfo propInfo in this.GetType().GetProperties())
            {
                if (propInfo.GetValue(this).Equals(other.GetType().GetProperty(propInfo.Name).GetValue(other)) == false)
                    return false;
            }
            return true;
        }
    }
}
