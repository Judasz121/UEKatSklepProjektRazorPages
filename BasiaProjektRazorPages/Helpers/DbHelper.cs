using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BasiaProjektRazorPages.Helpers

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
