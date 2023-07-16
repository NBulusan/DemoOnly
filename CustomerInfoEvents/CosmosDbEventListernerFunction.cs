using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CustomerInfoEvents
{
    public class CosmosDbEventListernerFunction
    {
        [FunctionName("ListenToCustomerChanges")]
        public void Run([CosmosDBTrigger(
            databaseName: "CustomerDb",
            collectionName: "Customers",
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "leases")] JArray documents,
            ILogger log)
        {
			foreach (var document in documents)
			{
				// Process the document (including updates and deletes)
				log.LogInformation($"Document ID: {document["id"]}, Operation: {document["operationType"]}");
			}
		}
    }
}
