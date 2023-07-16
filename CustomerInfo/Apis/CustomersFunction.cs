using CustomerInfo.Commands;
using CustomerInfo.Dto;
using CustomerInfo.Notifications;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerInfo.Apis
{
	public class CustomersFunction
	{
		private readonly IMediator _mediator;
		private readonly ILogger<CustomersFunction> _logger;
		private readonly IValidator<CustomerDto> _validator;
		public CustomersFunction(IMediator mediator, ILogger<CustomersFunction> logger, IValidator<CustomerDto> validator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

		[FunctionName("AddCustomer")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)] HttpRequest req)
		{

			try
			{
				string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
				var customerData = JsonConvert.DeserializeObject<CustomerDto>(requestBody);

				var customerValidationResult = await _validator.ValidateAsync(customerData);
				if (!customerValidationResult.IsValid)
				{
					var errorResult = customerValidationResult.Errors.Select(e => new
					{
						e.ErrorCode,
						e.PropertyName,
						e.ErrorMessage
					}).ToList();

					var formattedError = string.Join(Environment.NewLine, errorResult.Select(x => x.PropertyName + ": " + x.ErrorMessage));

					_logger.LogError("Input validation failed: {0}", formattedError);

					return new BadRequestObjectResult(errorResult);
				}

				var customerResponse = await _mediator.Send(new AddCustomerCommand(customerData));
				await _mediator.Publish(new CustomerAddedNotification(customerResponse));

				return new OkObjectResult(customerResponse);
			}
			catch (JsonException ex)
			{
				_logger.LogError("JSON parsing error: {0}", ex.StackTrace);
				return new BadRequestObjectResult("JSON parsing error!");
			}
		}
	}
}
