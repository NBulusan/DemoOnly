using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfo.Entities
{
	public class Customer
	{
		[JsonProperty("id")]
		public string Id { get; set; } = default!;
		[JsonProperty("firstName")]
		public string FirstName { get; set; } = default;
		[JsonProperty("lastName")]
		public string LastName { get; set; } = default;
		[JsonProperty("birtdayInEpoch")]
		public int BirthdayInEpoch { get; set; }
		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		[JsonProperty("lastUpdatedOn")]
		public DateTime LastUpdatedOn { get; set; }
	}
}
