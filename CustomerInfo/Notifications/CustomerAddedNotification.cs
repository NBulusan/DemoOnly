using CustomerInfo.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfo.Notifications
{
	public record CustomerAddedNotification(Customer Customer) : INotification;

}
