
using CustomerInfo.Dto;
using CustomerInfo.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfo.Validators
{
	public class CustomerDtoValidator : AbstractValidator<CustomerDto>
	{
		public CustomerDtoValidator()
		{
			var minDate = new DateTime(1970, 1, 1);
			var maxDate = DateTime.Now.Date.AddDays(-1);

			var minEpochDate = (new DateTimeOffset(minDate)).ToUnixTimeSeconds();
			var maxEpochDate = (new DateTimeOffset(maxDate)).ToUnixTimeSeconds();

			RuleFor(p=> p.Email).NotEmpty().EmailAddress();
			RuleFor(p=> p.FirstName).NotEmpty();
			RuleFor(p=> p.BirthdayInEpoch).NotEmpty()
				.Must(b=> b >= minEpochDate && b<= maxEpochDate)
				.WithMessage($"Birthday must be between {minEpochDate} and {maxEpochDate}");
		}
	}
}
