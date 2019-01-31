using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Interface
{
    public interface IHttpHelper
    {
        Task<string> Get(string url, string header, string body);
        Task<string> NoBaseGet(string url, string header);
        Task<string> Post<T>(string url, string header, T model) where T : class;
        Task<string> PostAsync(string url, string header, string json);
        Task<string> NoBasePost<T>(string methodName, string header, T model) where T : class;
        Task<string> PutAsync(string url, string header, string content);
    }
}
