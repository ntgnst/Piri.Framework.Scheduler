using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Interface
{
    public interface IHttpHelper
    {
        Task<string> Get(string url, List<KeyValuePair<string, string>> headerList, string body);
        Task<string> NoBaseGet(string url, List<KeyValuePair<string, string>> headerList);
        Task<string> Post<T>(string url, List<KeyValuePair<string, string>> headerList, T model) where T : class;
        Task<string> PostAsync(string url, List<KeyValuePair<string, string>> headerList, string json);
        Task<string> NoBasePost<T>(string methodName, List<KeyValuePair<string, string>> headerList, T model) where T : class;
    }
}
