using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CustomerInfoEvents
{
    public class CosmosDbEventListernerFunction
    {
		private readonly ICustomerEvents _customerEvents;

		public CosmosDbEventListernerFunction(ICustomerEvents customerEvents)
		{
			_customerEvents = customerEvents ?? throw new ArgumentNullException(nameof(customerEvents));
		}

		[FunctionName("ListenToCustomerChanges")]
        public async Task RunASync([CosmosDBTrigger(
            databaseName: "CustomerDb",
            collectionName: "Customers",
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "leases")] JArray documents)
        {
			foreach (var document in documents)
			{
				// Process the document (including updates and deletes)
				var customer = document.ToObject<Customer>();
				await _customerEvents.CustomerCreatedEvent(customer);
			}
		}
    }
}
