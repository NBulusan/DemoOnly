using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfo.Services
{
    public class CustomerInfoEventsHttpClient
    {
		private readonly HttpClient _httpClient;
		public CustomerInfoEventsHttpClient(HttpClient httpClient)
        {
			httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}
		public async Task<string> PostAsync(string url, string content)
		{
			HttpContent httpContent = new StringContent(content);
			HttpResponseMessage response = await _httpClient.PostAsync(url, httpContent);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}
	}
}
