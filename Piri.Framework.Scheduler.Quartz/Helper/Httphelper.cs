using Newtonsoft.Json;
using Piri.Framework.Scheduler.Quartz.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Helper
{
    public class HttpHelper : IHttpHelper
    {
        private string _responseBody;
        private HttpClient _client;
        private HttpResponseMessage _response;
        public async Task<string> Get(string url, List<KeyValuePair<string, string>> headerList, string body)
        {
            
            using (_client = new HttpClient())
            {
                try
                {
                    if (headerList!=null)
                    {
                        foreach (KeyValuePair<string, string> item in headerList)
                        {
                            _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }
                    _response = await _client.GetAsync(new Uri(url));
                    _responseBody = await _response.Content.ReadAsStringAsync();
                    await Console.Out.WriteLineAsync($"Replied from --> {url} \n  Response --> {_responseBody}");
                }
                catch (Exception ex)
                {
                    _responseBody = ex.ToString();
                }
                _client.Dispose();
                return _responseBody;
            }
        }
        public async Task<string> NoBaseGet(string url, List<KeyValuePair<string, string>> headerList)
        {
            using (_client = new HttpClient())
            {
                if (headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                _response = await _client.GetAsync(new Uri(url));
                _responseBody = await _response.Content.ReadAsStringAsync();
                _client.Dispose();
                return _responseBody;
            }
        }
        public async Task<string> Post<T>(string url, List<KeyValuePair<string, string>> headerList, T model) where T : class
        {
            using (_client = new HttpClient())
            {
                if (headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                _response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model)));
                _responseBody = await _response.Content.ReadAsStringAsync();
                _client.Dispose();
                return _responseBody;
            }
        }
        public async Task<string> NoBasePost<T>(string methodName, List<KeyValuePair<string, string>> headerList, T model) where T : class
        {
            using (_client = new HttpClient())
            {
                if (headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                _response = await _client.PostAsync(methodName, new StringContent(JsonConvert.SerializeObject(model)));
                _responseBody = await _response.Content.ReadAsStringAsync();
                _client.Dispose();
                return _responseBody;
            }
        }
        public async Task<string> PostAsync(string url, List<KeyValuePair<string, string>> headerList, string json)
        {
            using (_client = new HttpClient())
            {
                if (headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                _response = await _client.PostAsync(url, new StringContent(json));
                _responseBody = await _response.Content.ReadAsStringAsync();
                _client.Dispose();
                return _responseBody;
            }
        }
    }
}
