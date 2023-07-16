using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfoEvents
{
	public interface ICustomerEvents
	{
		Task CustomerCreatedEvent(Customer customer);
	}
}
