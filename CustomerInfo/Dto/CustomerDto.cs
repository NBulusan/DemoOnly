using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfo.Dto
{
	public class CustomerDto
	{
		public string FirstName { get; set; } = default;

		public string LastName { get; set; } = default;

		public int BirthdayInEpoch { get; set; }

		public string Email { get; set; }

	}
}
