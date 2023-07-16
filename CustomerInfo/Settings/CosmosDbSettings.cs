using CustomerInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfo.Settings
{
	public class CosmosDbSettings : ICosmosDbSettings
	{
		public string ConnectionString { get; set; }
	}
}
