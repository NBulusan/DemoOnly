using CustomerInfo.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerInfo.Handlers
{
	public class HttpCallHandler : INotificationHandler<CustomerAddedNotification>
	{
		public async Task Handle(CustomerAddedNotification notification, CancellationToken cancellationToken)
		{
			// do something here. 
			await Task.CompletedTask;
		}
	}
}
