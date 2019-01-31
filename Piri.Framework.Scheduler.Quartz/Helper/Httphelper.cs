using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        public HttpHelper(ILogger<HttpHelper> logger)
        {
            _logger = logger;
        }
        public async Task<string> Get(string url, List<KeyValuePair<string, string>> headerList, string body)
        {

            using (_client = new HttpClient())
            {
                try
                {
                    if (headerList != null && headerList.Any())
                    {
                        foreach (KeyValuePair<string, string> item in headerList)
                        {
                            _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }
                    _response = await _client.GetAsync(new Uri(url));
                    _responseBody = await _response.Content.ReadAsStringAsync();
                    await WriteResponse(url,_responseBody);
                }
                catch (Exception ex)
                {
                    _responseBody = ex.ToString();
                }
                _client.Dispose();
                return _responseBody;
            }
        }
        private async Task WriteResponse(string url,string responseBody)
        {
            _logger.LogInformation($"Replied from --> {url} \n  Response --> {_responseBody}");
        }
        public async Task<string> NoBaseGet(string url, List<KeyValuePair<string, string>> headerList)
        {
            using (_client = new HttpClient())
            {
                if (headerList != null && headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                _response = await _client.GetAsync(new Uri(url));
                _responseBody = await _response.Content.ReadAsStringAsync();
                await WriteResponse(url, _responseBody);
                _client.Dispose();
                return _responseBody;
            }
        }
        public async Task<string> Post<T>(string url, List<KeyValuePair<string, string>> headerList, T model) where T : class
        {
            using (_client = new HttpClient())
            {
                if (headerList != null && headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                _response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model)));
                _responseBody = await _response.Content.ReadAsStringAsync();
                _client.Dispose();
                await WriteResponse(url, _responseBody);
                return _responseBody;
            }
        }
        public async Task<string> NoBasePost<T>(string url, List<KeyValuePair<string, string>> headerList, T model) where T : class
        {
            using (_client = new HttpClient())
            {
                if (headerList != null && headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                _response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model)));
                _responseBody = await _response.Content.ReadAsStringAsync();
                _client.Dispose();
                await WriteResponse(url, _responseBody);
                return _responseBody;
            }
        }
        public async Task<string> PostAsync(string url, List<KeyValuePair<string, string>> headerList, string json)
        {
            using (_client = new HttpClient())
            {
                if (headerList != null && headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                _response = await _client.PostAsync(url, new StringContent(json));
                _responseBody = await _response.Content.ReadAsStringAsync();
                _client.Dispose();
                await WriteResponse(url, _responseBody);
                return _responseBody;
            }
        }
        public async Task<string> PutAsync(string url, List<KeyValuePair<string, string>> headerList, string content)
        {
            using (_client = new HttpClient())
            {
                if (headerList != null && headerList.Any())
                {
                    foreach (KeyValuePair<string, string> item in headerList)
                    {
                        _client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                _response = await _client.PostAsync(url, new StringContent(content));
                _responseBody = await _response.Content.ReadAsStringAsync();
                _client.Dispose();
                await WriteResponse(url, _responseBody);
                return _responseBody;
            }
        }

    }
}
