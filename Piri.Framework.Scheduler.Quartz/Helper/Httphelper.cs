using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Helper
{
    public class HttpHelperService
    {
        public async Task<string> Get(string url)
        {
            string responseBody;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    //client.DefaultRequestHeaders.Add("Token", Constants.GUID);
                    //client.DefaultRequestHeaders.Add("token", Constants.GUID);
                    HttpResponseMessage response = await client.GetAsync(new Uri(url));
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    responseBody = ex.ToString();
                }
                return responseBody;
            }
        }

        public async Task<string> NoBaseGet(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                //client.DefaultRequestHeaders.Add("Token", Constants.GUID);

                HttpResponseMessage response = await client.GetAsync(new Uri(url));
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
        }

        public async Task<string> Post<T>(string url, T model) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                //client.DefaultRequestHeaders.Add("Token", Constants.GUID);
                //, Encoding.UTF8, "application/json"
                HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model)));

                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
        }

        public async Task<string> NoBasePost<T>(string methodName, T model) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                //client.DefaultRequestHeaders.Add("Token", Constants.GUID);
                //, Encoding.UTF8, "application/json"
                HttpResponseMessage response = await client.PostAsync(methodName, new StringContent(JsonConvert.SerializeObject(model)));

                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
        }
    }
}
