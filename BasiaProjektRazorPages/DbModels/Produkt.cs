using System.Text.RegularExpressions;

namespace BasiaProjektRazorPages.DbModels
{
    public class Produkt : BaseDbModel
    {
        public int ID_Produktu { get; set; }
        public string Nazwa { get; set; }
        public int? Cena_jednostkowa { get; set; }
        public int? ID_Kategorii { get; set; }

        public static Tuple<bool, string> VerifyValues(string name, int? price)
        {
            Regex nonDigit = new Regex(@"/D");
            bool ok = true;
            string msg = "";
            // name
            if(name != null && (string.IsNullOrWhiteSpace(name)))
            {
                ok = false;
                msg = "Nazwa produktu jest pusta";
            }

            // price
            float result;
            if(price != null && (price < 0))
            {
                ok = false;
                if (!string.IsNullOrEmpty(msg))
                    msg += "\n";
                msg += "Cena nie może być ujemna";
            }

            return new Tuple<bool, string>(ok, msg);
        }
    }
}
