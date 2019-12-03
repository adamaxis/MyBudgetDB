using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace XUnitTestMyBudgetDB.Helpers
{
    class HomeTestFixture : IDisposable
    {
        //public HomeTestFixture() 
        //{
        //    var projectRootPath = Path.Combine(
        //        Directory.GetCurrentDirectory(),
        //        "..", "..", "..", "..", "..", "MyBudget", "MyBudget");

        //    var builder = Program.CreateWebHostBuilder()
        //        .UseContentRoot(projectRootPath);

        //    Server = new TestServer(builder);

        //    Client = Server.CreateClient();
        //    Client.BaseAddress = new Uri("http://localhost");
        //}

        //public HttpClient Client { get; }
        //public TestServer Server { get; }

        //public void Dispose()
        //{
        //    //Client?.Dispose();
        //    //Server?.Dispose();
        //}
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
