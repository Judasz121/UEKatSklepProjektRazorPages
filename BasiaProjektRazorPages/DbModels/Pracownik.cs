using System.ComponentModel;
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


        [DisplayName("Magazyn")]
        public int ID_Magazynu { get; set; }

        public int Wyplata { get; set; }

        public string Numer_telefonu { get; set; }

        public string PESEL { get; set; }

        public string Numer_konta { get; set; }

        public DateTime Data_Zatrudnienia { get; set; }   

        public DateTime Data_Zwolnienia { get; set; }
        public static Tuple<bool, string> VerifyValues(string? Imie, string? Nazwisko, int? Wyplata, string? Numer_telefonu,string? PESEL, string? Numer_konta)
        {
            bool ok = true;
            string msg = "";
            Regex nonDigit = new Regex(@"/D");
            Regex nonLetter = new Regex(@"[^a-zA-Z]");
            if (Imie != null)
            {
                if (string.IsNullOrWhiteSpace(Imie))
                {
                    ok = false;
                    msg += "Imie nie może być pusta";
                }
                if (nonLetter.IsMatch(Imie))
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Imię zawiera znaki niedozwolone";                    
                }
            }
  
            if (Nazwisko != null)
            {
                if (string.IsNullOrWhiteSpace(Nazwisko))
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Nazwisko nie może być puste";
                }
                if (nonLetter.IsMatch(Nazwisko))
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Nazwisko zawiera znaki niedozwolone";
                }
            }

            if(PESEL != null) {
                if (string.IsNullOrWhiteSpace(PESEL))
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Pesel nie może być pusty.";
                }
                else if (PESEL.Length != 11)
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "PESEL ma nieprawidłową długość";
                }
            }

            if (Wyplata != null)
            {
                if(Wyplata.Value == default)
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Wyplata nie może być pusta.";
                }
                if (Wyplata <= 3100)
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Taka wypłata tylko na czarno.";
                }
            }
          
            if (Numer_telefonu != null)
            {
                if (string.IsNullOrWhiteSpace(Numer_telefonu))
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Numer telefonu nie może być pusty";
                }
                if (nonDigit.IsMatch(Numer_telefonu))
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Numer mo niedozwolone znaki.";
                }
                if (Numer_telefonu.Length > 9)
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Numer jest za długi.";
                }
            }

            if (Numer_konta != null)
            {
                if (string.IsNullOrWhiteSpace(Numer_konta))
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Numer konta jest pusty.";
                }               
                if (Numer_konta.Length > 26)
                {
                    ok = false;
                    msg += "Numer konta jest za długi.";
                }
            }

            return new Tuple<bool, string>(ok, msg);
        }
        public Tuple<bool, string> VerifyInstanceValues(bool ignoreNullProps = false)
        {
            Pracownik toVerify = (Pracownik)this.MemberwiseClone();
            if (!ignoreNullProps)
                toVerify.changeNullValueTypePropertiesToDefaultValues();
            return Pracownik.VerifyValues(toVerify.Imie, toVerify.Nazwisko, toVerify.Wyplata, toVerify.Numer_telefonu, toVerify.PESEL, toVerify.Numer_konta);
        }
    }
}
