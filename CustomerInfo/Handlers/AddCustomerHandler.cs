using CustomerInfo.Commands;
using CustomerInfo.Entities;
using CustomerInfo.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerInfo.Handlers
{
	public class AddCustomerHandler : IRequestHandler<AddCustomerCommand, Customer> 
	{
		private readonly ICustomerRepository _customerRepository;
		public AddCustomerHandler(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
		} 

		public async Task<Customer> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
		{
			var response = await _customerRepository.CreateAsync(request.Customer, cancellationToken);

			return response;
		}
	}
}
