using AutoMapper;
using CustomerInfo.Apis;
using CustomerInfo.Interfaces;
using CustomerInfo.Repositories;
using CustomerInfo.Services;
using CustomerInfo.Settings;
using CustomerInfo.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(CustomerInfo.Startup))]
namespace CustomerInfo
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

			// Register MediatR
			builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

			// Register validators
			builder.Services.AddValidatorsFromAssemblyContaining<CustomerDtoValidator>();

			// Register Automapper Profile
			builder.Services.AddAutoMapper(Assembly.GetAssembly(this.GetType()));

			var cosmosDbSettings = new CosmosDbSettings();
			configuration.Bind("CosmosDb", cosmosDbSettings);
			builder.Services.AddSingleton(cosmosDbSettings);

			this.ProvisionCosmosDbAndContainers(cosmosDbSettings).GetAwaiter().GetResult();

			builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
			builder.Services.AddTransient<CustomersFunction>();

			// Register http client
			//builder.Services.AddSingleton<CustomerInfoEventsHttpClient>(provider =>
			//{
			//	var httpClient = new HttpClient { BaseAddress = new Uri("https://api.example.com/") };
			//	return new CustomerInfoEventsHttpClient(httpClient);
			//});

			builder.Services.AddLogging();
		}

		private async Task ProvisionCosmosDbAndContainers(CosmosDbSettings cosmosDbSettings)
		{
			CosmosClient cosmosClient = new CosmosClient(cosmosDbSettings.ConnectionString);
			Microsoft.Azure.Cosmos.Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync("CustomerDb");
			var container = await database.CreateContainerIfNotExistsAsync("Customers", "/id");

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
