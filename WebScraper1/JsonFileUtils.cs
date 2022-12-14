using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebScraper1
{
    public static class JsonFileUtils
    {
        private static readonly JsonSerializerOptions _options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public static void SimpleWrite(object obj, string fileName)
        {
            var jsonString = System.Text.Json.JsonSerializer.Serialize(obj, _options);
            File.WriteAllText(fileName, jsonString);
        }
    }

    //public static class JsonFileUtils
    //{
    //    private static readonly JsonSerializerSettings _options
    //        = new() { NullValueHandling = NullValueHandling.Ignore };

    //    public static void SimpleWrite(object obj, string fileName)
    //    {
    //        var jsonString = JsonConvert.SerializeObject(obj, _options);
    //        File.WriteAllText(fileName, jsonString);
    //    }
    //}
}