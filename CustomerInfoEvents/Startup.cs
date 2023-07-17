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
			/*
			builder.Services.AddOptions<CosmosDBOptions>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("CosmosDb").Bind(settings);
				});
			*/

			var cosmosDbSettings = new CosmosDbSettings();
			configuration.Bind("CosmosDb", cosmosDbSettings);
			builder.Services.AddSingleton(cosmosDbSettings);

			this.ProvisionCosmosDbAndContainers(cosmosDbSettings).GetAwaiter().GetResult();

			builder.Services.AddTransient<ICustomerEvents, CustomerEvents>();

			var eventGridSettings = new EventGridSettings();
			configuration.Bind("EventGrid", eventGridSettings);
			builder.Services.AddSingleton(eventGridSettings);
			builder.Services.AddLogging();

		}

		private async Task ProvisionCosmosDbAndContainers(CosmosDbSettings cosmosDbSettings)
		{
			CosmosClient cosmosClient = new CosmosClient(cosmosDbSettings.ConnectionString);
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
