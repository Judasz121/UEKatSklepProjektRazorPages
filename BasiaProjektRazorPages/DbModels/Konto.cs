using System.Text.RegularExpressions;
using Dapper;
using System.Data;
using BasiaProjektRazorPages.Helpers;
using System.Reflection;

namespace BasiaProjektRazorPages.DbModels
{
    public class Konto : BaseDbModel
    {
        public int? ID_Konta { get; set; }
        public int? ID_Klienta { get; set; }
        public int? ID_Dostawcy { get; set; }
        public string LoginUzytkownika { get; set; }
        public string Email { get; set; }
        public string HashHasla { get; set; }
        public bool JestAdminem { get; set; }
        public DateTime DataUtworzenia { get; set; }

        public static Tuple<bool, string> verifyValues(string? login, string? password, string? email)
        {
            bool ok = true;
            string msg = "";
            Regex mailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");

            // login
            if (login != null)
            {
                if (string.IsNullOrWhiteSpace(login) || login.Length < 6)
                {
                    ok = false;
                    msg = "Nazwa użytkownika musi mieć conajmniej 6 znaków.";
                }
                int nameTaken = 0;
                using (IDbConnection conn = DbHelper.GetDbConnection())
                    nameTaken = conn.ExecuteScalar<int>($"SELECT COUNT(*) FROM Konto WHERE LoginUzytkownika = '{login}'");
                if (nameTaken > 0)
                {
                    ok = false;
                    if (string.IsNullOrEmpty(msg) == false)
                        msg += "\n";
                    msg += "Ta nazwa użytkownika jest już zajęta.";
                }
            }
            // password
            if ((string.IsNullOrWhiteSpace(password) || password.Length < 6) && password != null)
            {
                ok = false;                
                if (string.IsNullOrEmpty(msg) == false)
                    msg += "\n";
                msg += "Hasło musi mieć conajmniej 6 znaków.";
            }
            // e-mail
            if ((string.IsNullOrWhiteSpace(email) || mailRegex.IsMatch(email) == false) && email != null)
            {
                ok = false;
                if (string.IsNullOrEmpty(msg) == false)
                    msg += "\n";
                msg += "E-mail jest nieprawidłowy. 😡";
            }


            return new Tuple<bool, string>(ok, msg);
        }
        public Tuple<bool, string> VerifyInstanceValues(bool ignoreNullProps = false)
        {
            Konto toVerify = (Konto)this.MemberwiseClone();
            if (ignoreNullProps == false)
            {
                toVerify.changeNullPropertiesToDefaultValues();
            }
            return Konto.verifyValues(toVerify.LoginUzytkownika, toVerify.HashHasla, toVerify.Email);            
        }


    }
}
