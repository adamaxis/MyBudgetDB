using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyBudgetDB;
using Xunit;

namespace XUnitTestMyBudgetDB
{
    public class TestGlobal
    {
        private IOptions<AppSecrets> _configSecrets;

        [Fact]
        public void GlobalPrepare()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();

            _configSecrets = Options.Create(configuration.GetSection("MyBudgetDB").Get<AppSecrets>());
        }

        //[Fact]
        //public async Task StatusMiddleware()
        //{
        //    var hostBuilder = new WebHostBuilder()
        //        .UseStartup<Startup>();

        //    using (var server = new TestServer(hostBuilder))
        //    {
        //        HttpClient client = server.CreateClient();
        //        var response = await client.GetAsync("Index");
        //        response.EnsureSuccessStatusCode();
        //    }
            
        //}
    }
}
