using System.Text.RegularExpressions;
namespace BasiaProjektRazorPages.DbModels
{
    public class Dostawca : BaseDbModel
    {
        public int? ID_Dostawcy { get; set; }  

        public string Nazwa { get; set; }

        public string Numer_telefonu { get; set; }

        public string Email { get; set; }

        public string NIP { get; set; }

        public static Tuple<bool, string> VerifyValues(string name, string phone, string email, string nip)
        {
            bool ok = true;
            string msg = "";
            Regex mailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
            Regex nonDigit = new Regex(@"/D");
            Regex nonLetter = new Regex(@"[^a-zA-Z]");

            // name
            if(name != null)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    ok = false;
                    msg = "Scheiße! ze name ist puste!";
                }
            }
            // phone
            if (phone != null)
            {
                if ((string.IsNullOrWhiteSpace(phone) || nonDigit.IsMatch(phone)))
                {
                    ok = false;
                    if (!string.IsNullOrWhiteSpace(msg))
                        msg += "\n";
                    msg += "Telefon zawiera znaki niedozwolone lub wogle nie zawiera znaków 😠";
                }
            }
            // e-mail
            if ((string.IsNullOrWhiteSpace(email) || mailRegex.IsMatch(email) == false) && email != null)
            {
                ok = false;
                if (string.IsNullOrWhiteSpace(msg) == false)
                    msg += "\n";
                msg += "E-mail jest nieprawidłowy lub go nima 😡";
            }
            // nip
            if(nip != null && (string.IsNullOrWhiteSpace(nip) || nonDigit.IsMatch(nip)))
            {
                ok = false;
                if (!string.IsNullOrWhiteSpace(msg))
                    msg += "\n";
                msg += "Nieprawidłowy lub brak NIPka";
            }

            return new Tuple<bool, string>(ok, msg);
        }
        public Tuple<bool,string> VerifyInstanceValues(bool ignoreNullProps = false)
        {
            Dostawca toVerify = (Dostawca)this.MemberwiseClone();
            if(!ignoreNullProps)
                toVerify.changeNullValueTypePropertiesToDefaultValues();
            return Dostawca.VerifyValues(toVerify.Nazwa, toVerify.Numer_telefonu, toVerify.Email, toVerify.NIP);

        }
    }
}
