using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Interface
{
    public interface IHttpHelper
    {
        Task<string> Get(string url, string header, string body, CancellationToken cancellationToken = default(CancellationToken));
        Task<string> NoBaseGet(string url, string header, CancellationToken cancellationToken = default(CancellationToken));
        Task<string> Post<T>(string url, string header, T model, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
        Task<string> PostAsync(string url, string header, string json, CancellationToken cancellationToken = default(CancellationToken));
        Task<string> NoBasePost<T>(string methodName, string header, T model, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
        Task<string> PutAsync(string url, string header, string content, CancellationToken cancellationToken = default(CancellationToken));
    }
}
