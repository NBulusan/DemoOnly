using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(CustomerInfoEvents.Startup))]
namespace CustomerInfoEvents
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Environment.CurrentDirectory)
				.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();

			builder.Services.AddOptions<CosmosDBOptions>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("CosmosDb").Bind(settings);
				});

			var cosmosDbSettings = configuration.GetSection("CosmosDb").GetValue<string>("ConnectionString");
			ProvisionCosmosDbAndContainers(cosmosDbSettings).GetAwaiter().GetResult();
		}

		private async Task ProvisionCosmosDbAndContainers(string connectiongString)
		{
			CosmosClient cosmosClient = new CosmosClient(connectiongString);
            Microsoft.Azure.Cosmos.Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync("CustomerDb");
			await database.CreateContainerIfNotExistsAsync("Customers", "/id");

			await cosmosClient.GetDatabase("CustomerDb")
				.DefineContainer(name: "Customers", partitionKeyPath: "/id")
				.WithUniqueKey()
				.Path("/email")
				.Attach()
				.CreateIfNotExistsAsync();

			await database.CreateContainerIfNotExistsAsync("leases", "/id");
		}
	}
}
