using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Helper
{
    public class HttpHelperService
    {
        public async Task<string> Get(string url, List<KeyValuePair<string, string>> headerList, string body)
        {
            string responseBody;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (headerList.Any())
                    {
                        foreach (KeyValuePair<string, string> item in headerList)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }
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
        public async Task<string> NoBaseGet(string url, List<KeyValuePair<string, string>> headerList)
        {
            using (HttpClient client = new HttpClient())
            {
                if (headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                HttpResponseMessage response = await client.GetAsync(new Uri(url));
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
        }
        public async Task<string> Post<T>(string url, List<KeyValuePair<string, string>> headerList, T model) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                if (headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model)));
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
        }
        public async Task<string> NoBasePost<T>(string methodName, List<KeyValuePair<string, string>> headerList, T model) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                if (headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                HttpResponseMessage response = await client.PostAsync(methodName, new StringContent(JsonConvert.SerializeObject(model)));
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
        }
    }
}
