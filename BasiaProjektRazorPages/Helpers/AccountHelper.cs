using BasiaProjektRazorPages.DbModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;

namespace BasiaProjektRazorPages.Helpers
{
    public static class AccountHelper
    {
        public static Regex mailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        public static bool loggedInVerified = false;
        public static Konto loggedInAccount = null;

        #region passwordHashing
        private static int keySize = 64;
        private static int iterations = 350000;
        public static string hashPassword(string password, string salt)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                                                Encoding.UTF8.GetBytes(password),
                                                saltBytes,
                                                iterations,
                                                hashAlgorithm,
                                                keySize
                                                );
            return Convert.ToHexString(hash);
        }
        public static bool verifyPassword(string password, string hash, string salt)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            string pwdHash = hashPassword(password, salt);
            return pwdHash.SequenceEqual(hash);
        }
        #endregion passwordHashing

        public static Tuple<bool, string> checkForLoggedInSession(HttpContext context, bool updateAccountHelperLoggedInAccountFields = true)
        {
            string accId = context.Session.GetString("loggedInAccountId");
            string pwdHash = context.Session.GetString("loggedInAccountPasswordHash");
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                Konto account = null;
                try
                {
                    account = conn.QueryFirst<Konto>($"SELECT TOP 1 * FROM Konto WHERE ID_Konta = '{accId}'");
                }
                catch (InvalidOperationException ex)
                {
                    if (account == null)
                        return new Tuple<bool, string>(false, "Account not found");
                }

                if (account.HashHasla != pwdHash)
                    return new Tuple<bool, string>(false, "Wrong password in session");

                if (updateAccountHelperLoggedInAccountFields)
                {
                    AccountHelper.loggedInAccount = account;
                    AccountHelper.loggedInVerified = true;
                }

                return new Tuple<bool, string>(true, null);
            }

        }

        public static void Logout(HttpContext context)
        {
            context.Session.Remove("loggedInAccountId");
            context.Session.Remove("loggedInAccountPasswordHash");
            AccountHelper.loggedInAccount = null;
            AccountHelper.loggedInVerified = false;
        }
    }
}
