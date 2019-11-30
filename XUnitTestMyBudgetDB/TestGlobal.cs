using System.IO;
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
    }
}
