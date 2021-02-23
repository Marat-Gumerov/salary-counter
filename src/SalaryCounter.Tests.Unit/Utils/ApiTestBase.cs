using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SalaryCounter.Api;

namespace SalaryCounter.ServiceTest.Utils
{
    [TestFixture]
    public abstract class ApiTestBase
    {
        protected HttpClient Client = null!;

        [OneTimeSetUp]
        public void SetUpClient()
        {
            Client = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => builder
                    .UseUrls("http://*:53435")
                    .UseEnvironment("NoSpa")
                    .ConfigureTestServices(SetMocks))
                .CreateClient(new WebApplicationFactoryClientOptions
                    {BaseAddress = new Uri("http://localhost:53435/api/v1/")});
        }
        
        protected abstract void SetMocks(IServiceCollection services);

        [OneTimeTearDown]
        public void TearDownClient()
        {
            Client.Dispose();
        }
    }
}