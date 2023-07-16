using AutoMapper;
using AutoMapper.Internal.Mappers;
using Azure.Core;
using CustomerInfo.Dto;
using CustomerInfo.Entities;
using CustomerInfo.Interfaces;
using CustomerInfo.Settings;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerInfo.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly CosmosClient _cosmosClient;
		private readonly Container _container;
		private readonly CosmosDbSettings _cosmosDbSettings;
		private readonly IMapper _mapper;

		public CustomerRepository(CosmosDbSettings cosmosDbSettings, IMapper mapper)
		{
			if (cosmosDbSettings == null && cosmosDbSettings.ConnectionString == null)
			{
				throw new ArgumentNullException(nameof(cosmosDbSettings));
			}

			_cosmosClient = new CosmosClient(cosmosDbSettings.ConnectionString);
			_container = _cosmosClient.GetContainer("CustomerDb", "Customers");
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<Customer> CreateAsync(CustomerDto customer, CancellationToken ct)
		{
			var newCustomer = _mapper.Map<CustomerDto, Customer>(customer);
			newCustomer.Id = Guid.NewGuid().ToString();
			var itemResponse = await _container.CreateItemAsync(newCustomer, new PartitionKey(newCustomer.Id), cancellationToken: ct);

			return itemResponse.Resource;
		}
	}
}
