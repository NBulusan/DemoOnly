using Microsoft.Azure.EventGrid.Models;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;

namespace CustomerInfoEvents 
{
	public class CustomerEvents : ICustomerEvents
	{
		TopicCredentials credentials;
		EventGridClient client;
		string topicHostname;
		private readonly EventGridSettings _eventGridSettings;

		public CustomerEvents(EventGridSettings eventGridSettings)
		{
			_eventGridSettings = eventGridSettings ?? throw new ArgumentNullException(nameof(eventGridSettings));

			if (string.IsNullOrEmpty(_eventGridSettings.ApiKey) && string.IsNullOrEmpty(_eventGridSettings.EndpointURL))
			{
				throw new ArgumentNullException(nameof(_eventGridSettings));
			}

			credentials = new TopicCredentials(_eventGridSettings.ApiKey);
			client = new EventGridClient(credentials);
			topicHostname = new Uri(_eventGridSettings.EndpointURL).Host;
		}

		public async Task CustomerCreatedEvent(Customer customer)
		{
			List<EventGridEvent> eventsList = new List<EventGridEvent>();
			var eventGridEvent = new EventGridEvent()
			{
				Id = Guid.NewGuid().ToString(),
				EventType = "Demo.Customers.Changed",
				Data = customer,
				EventTime = DateTime.UtcNow,
				Subject = customer.Id,
				DataVersion = "1.0"
			};
			eventsList.Add(eventGridEvent);
			await client.PublishEventsAsync(topicHostname, eventsList);
		}

	}
}
