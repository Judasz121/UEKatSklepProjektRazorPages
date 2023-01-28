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
                if (string.IsNullOrWhiteSpace(Kraj))
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
                if (string.IsNullOrWhiteSpace(Miasto))
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
                if(string.IsNullOrWhiteSpace(Ulica))
                {
                    ok= false;
                    msg += "Nazwa Ulicy nie może być pusta";
                }
                if(Ulica.Length > 20)
                {
                    ok = false;
                    msg += "Taka nazwa Ulicy nie istnieje";
                }
            }
            //Kod_pocztowy
            if (Kod_pocztowy != null)
            {
                if (string.IsNullOrWhiteSpace(Kod_pocztowy))
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
                if (string.IsNullOrWhiteSpace(Numer_budynku))
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
            //Numer mieszkania
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


        public Tuple<bool, string> VerifyInstanceValues(bool ignoreNullProps = false)
        {
            if(ignoreNullProps)
                return Adres.verifyAddress(this.Kraj, this.Miasto, this.Ulica, this.Kod_pocztowy, this.Numer_budynku, this.Numer_mieszkania);
            else
            {
                Adres temp = (Adres)this.MemberwiseClone();
                temp.changeNullValueTypePropertiesToDefaultValues();
                return Adres.verifyAddress(temp.Kraj, temp.Miasto, temp.Ulica, temp.Kod_pocztowy, temp.Numer_budynku, temp.Numer_mieszkania);
            }
        }
    }
}
