using Microsoft.Extensions.Configuration;

namespace client.Pages.Config
{
    public class ServerConfig
    {
        public string Domain { get; }

        public ServerConfig()
        {
            var conf = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
            Domain = conf["Server:Domain"];
        }
    }
}
