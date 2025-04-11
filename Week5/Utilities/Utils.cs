using System.Text.Json;

/*  "Create a Utils class, define two methods inside this class that export data in JSON format. 
The first method takes any type of data and converts it to a JSON string takes the
JSON data and saves it to a specific file path.*/
namespace Week5.Utilities
{
    public  class Utils
    {
        
        private static readonly Lazy<Utils> instance = new Lazy<Utils>(() => new Utils());
   
        public static Utils Instance => instance.Value;

        private Utils() { }




         public void ExportToJsonFile<T>(IEnumerable<T> data, string filePath)
        {
            string json = ExportToJson(data);
            File.WriteAllText(filePath, json);
        }

        public string ExportToJson<T>(IEnumerable<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(data, options);
        }


    }
}