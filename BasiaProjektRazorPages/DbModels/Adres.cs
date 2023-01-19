using System.Security.Cryptography;

namespace BasiaProjektRazorPages.DbModels
{
    public class Adres : BaseDbModel
    {
        public int ID_Adresu { get; set; }
        public int ID_Klienta { get; set; }
        public string Kraj { get; set; }
        public string Miasto { get; set; }
        public string  Ulica { get; set; }
        public string Kod_pocztowy { get; set; }
        public  string Numer_budynku { get; set; }
        public string Numer_mieszkania { get; set; }

        public static Tuple<bool, string> verifyAddress(string? Kraj, string? Miasto, string? Ulica, string? Kod_pocztowy, string? Numer_budynku, string? Numer_mieszkania)
        {
            bool ok = true;
            string msg = "";
            //Kraj
            if (Kraj != null)
            {
                if (string.IsNullOrEmpty(Kraj))
                {
                    ok = false;
                    msg += "Nazwa Kraju nie może być pusta";
                }
                if(Kraj.Length > 20)
                {
                    ok = false;
                    msg += "Taka nazwa kraju nie istnieje.";
                }
            }
            //Miasto
            if(Miasto != null)
            {
                if (string.IsNullOrEmpty(Miasto))
                {
                    ok = false;
                    msg += "Nazwa Miasta nie może być pusta";
                }
                if(Miasto.Length > 20)
                {
                    ok = false;
                    msg += "Taka nazwa miasta nie istnieje";
                }
            }
            //Ulica
            if(Ulica != null)
            {
                if(string.IsNullOrEmpty(Ulica))
                {
                    ok= false;
                    msg += "Nazwa Ulicy nie może być pusta";
                }
                if(Miasto.Length > 20)
                {
                    ok = false;
                    msg += "Taka nazwa Ulicy nie istnieje";
                }
            }
            //Kod_pocztowy
            if (Kod_pocztowy != null)
            {
                if (string.IsNullOrEmpty(Kod_pocztowy))
                {
                    ok = false;
                    msg += "Kod pocztowy nie może być pusty";
                }
                if(Kod_pocztowy.Length > 10)
                {
                    ok = false;
                    msg += "Taki kod pocztowy nie istnieje";
                }
            }
            //Numer budynku
            if (Numer_budynku != null)
            {
                if (string.IsNullOrEmpty(Numer_budynku))
                {
                    ok = false;
                    msg += "Numer_budynku nie może być pusty";
                }
                if(Numer_budynku.Length > 3)
                {
                    ok = false;
                    msg += "Taki numer budynku nie istnieje";
                }
            }
            if (Numer_mieszkania == null)
            {
                ok = true;
            }
            else if(Numer_mieszkania.Length > 2)
            {
                ok = false;
                msg += "Taki numer mieszkania nie istnieje.";
            }

            return new Tuple<bool, string>(ok, msg);
        }
    }
}
