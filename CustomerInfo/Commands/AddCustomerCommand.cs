
using CustomerInfo.Dto;
using CustomerInfo.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfo.Commands
{
	public record AddCustomerCommand(CustomerDto Customer) : IRequest<Customer>;

}
