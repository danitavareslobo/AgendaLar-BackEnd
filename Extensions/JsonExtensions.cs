using System.Text.Json;

namespace AgendaLarAPI.Extensions
{
    public static class JsonExtensions
    {
        public static JsonSerializerOptions JsonOptions => GetJsonOptions();

        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
    }
}
