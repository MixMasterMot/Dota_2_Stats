using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dota_2_Stats.API
{
    public class RequestHandler
    {
        const int MaxRetries = 3;
        public string GET(string url)
        {
            //return GETAsync(url, 0);
            return "";
        }

        public async Task<string> GETAsync(string url, int retries = 3)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var s = await response.Content.ReadAsStringAsync();
                    return s;
                }
            }
            return null;
        }
    }
}
