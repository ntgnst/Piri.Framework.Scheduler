﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Piri.Framework.Scheduler.Quartz.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Helper
{
    public class HttpHelper : IHttpHelper
    {
        private string _responseBody;
        private HttpClient _client;
        private HttpResponseMessage _response;
        private readonly ILogger _logger;
        private Dictionary<string, string> _headers;
        public HttpHelper(ILogger<HttpHelper> logger)
        {
            _logger = logger;
        }
        public async Task<string> Get(string url, string header, string body, CancellationToken cancellationToken = default(CancellationToken))
        {

            using (_client = new HttpClient())
            {
                try
                {
                    ConvertAndAddHeader(_client, header);
                    _response = await _client.GetAsync(new Uri(url));
                    _responseBody = await _response.Content.ReadAsStringAsync();
                    await WriteResponse(url, _responseBody);
                }
                catch (Exception ex)
                {
                    _responseBody = ex.ToString();
                }
                return _responseBody;
            }
        }
        public async Task<string> NoBaseGet(string url, string header, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (_client = new HttpClient())
            {
                try
                {
                    ConvertAndAddHeader(_client, header);
                    _response = await _client.GetAsync(new Uri(url));
                    _responseBody = await _response.Content.ReadAsStringAsync();
                    await WriteResponse(url, _responseBody);
                }
                catch (Exception ex)
                {
                    _responseBody = ex.ToString();
                    _logger.LogError($"HttpHelper.NoBaseGet Method Ex:{ex}");
                }
                
                return _responseBody;
            }
        }
        public async Task<string> Post<T>(string url, string header, T model, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            using (_client = new HttpClient())
            {
                ConvertAndAddHeader(_client, header);
                _response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model)));
                _responseBody = await _response.Content.ReadAsStringAsync();
                await WriteResponse(url, _responseBody);
                return _responseBody;
            }
        }
        public async Task<string> NoBasePost<T>(string url, string header, T model, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            using (_client = new HttpClient())
            {
                ConvertAndAddHeader(_client, header);
                _response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model)));
                if (cancellationToken.IsCancellationRequested)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError).ToString();
                }
                _responseBody = await _response.Content.ReadAsStringAsync();
                await WriteResponse(url, _responseBody);
                return _responseBody;
            }
        }
        public async Task<string> PostAsync(string url, string header, string json, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (_client = new HttpClient())
            {
                try
                {
                    ConvertAndAddHeader(_client, header);
                    _response = await _client.PostAsync(url, new StringContent(json), cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new HttpResponseMessage(HttpStatusCode.InternalServerError).ToString();
                    }
                    _responseBody = await _response.Content.ReadAsStringAsync();
                    await WriteResponse(url, _responseBody);
                }
                catch (Exception ex)
                {
                    _responseBody = ex.ToString();
                }

            }
            return _responseBody;
        }
        public async Task<string> PutAsync(string url, string header, string content, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (_client = new HttpClient())
            {
                ConvertAndAddHeader(_client, header);
                _response = await _client.PostAsync(url, new StringContent(content));
                _responseBody = await _response.Content.ReadAsStringAsync();
                await WriteResponse(url, _responseBody);
            }
            return _responseBody;
        }
        private void ConvertAndAddHeader(HttpClient client, string header)
        {
            if (!string.IsNullOrEmpty(header))
            {
                try
                {
                    _headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(header);
                    foreach (KeyValuePair<string, string> headerPair in _headers)
                    {
                        client.DefaultRequestHeaders.Add(headerPair.Key, headerPair.Value);
                    }
                }
                catch
                {
                    return;
                }
            }
        }
        private async Task WriteResponse(string url, string responseBody)
        {
            _logger.LogInformation($"Replied from --> {url} \n  Response --> {_responseBody}");
            await Console.Out.WriteLineAsync($"Replied from --> {url} \n  Response --> {_responseBody}");
        }

    }
}
