using Newtonsoft.Json;
using System.IO;

namespace TestBot.Configuration
{
    class Configurator
    {
        public static T GetConfiguration<T>(string configPath)
        {
            string jsonString;
            using (var file = new StreamReader(configPath))
            {
                jsonString = file.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
