using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SklepProjektRazorPages.Helpers

{
    public static class DbHelper
    {
        public static string GetConnectionString(string name = "Default")
        {
            string json = File.ReadAllText("appsettings.json");
            JsonDocument config = JsonSerializer.Deserialize<JsonDocument>(json, new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            });
            return config.RootElement.GetProperty("ConnectionStrings").GetProperty(name).GetString();
        }
        public static IDbConnection GetDbConnection(string connectionString = null)
        {
            if (connectionString == null)
                connectionString = DbHelper.GetConnectionString();
            return new System.Data.SqlClient.SqlConnection(connectionString);
        }

        public static string ReplacePolishChars(string target)
        {
            return target
                .Replace('ą', 'a')
                .Replace('Ą', 'A')
                .Replace('ć', 'c')
                .Replace('Ć', 'C')
                .Replace('ę', 'e')
                .Replace('Ę', 'E')
                .Replace('ł', 'l')
                .Replace('Ł', 'L')
                .Replace('ó', 'o')
                .Replace('Ó', 'O')
                .Replace('ś', 's')
                .Replace('Ś', 'S')
                .Replace('ż', 'z')
                .Replace('Ż', 'Z')
                .Replace('ź', 'z')
                .Replace('Ź', 'Z');
        }

        public static string absoluteImageStorageFolderPath
        {
            get
            {
                string rootPath = Directory.GetCurrentDirectory();
                return Path.Combine(rootPath, relativeImageStorageFolderPath);
            }
        }
        public static string relativeImageStorageFolderPath = @"wwwroot\DbImages";
    }
}
