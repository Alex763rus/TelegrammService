
using TelegrammService.model;

namespace TelegrammService.service
{
    public class ConfigService
    {
        public Config getConfig()
        {
            Config config = new Config();
            try
            {
                using (StreamReader reader = new StreamReader("config.txt"))
                {
                    config.url = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if(config.url == null)
            {
                config.url = "http://localhost:8039";
            }
            return config;
        }

    }
}
