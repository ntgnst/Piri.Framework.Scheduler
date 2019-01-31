using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Extension;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz
{
    public class SimpleTestProcess : PiriJob
    {
        private readonly IJobService _jobService;
        private readonly IHttpHelper _httpHelper;
        private Result<JobDto> _result;
        private JobDataDto _jobDataDto;
        public SimpleTestProcess(IJobService jobService, IHttpHelper httpHelper)
        {
            _jobService = jobService;
            _httpHelper = httpHelper;
        }
        public override async Task StartAsync(IJobExecutionContext context)
        {
            //Logic : HttpRequests will be sent this section , Job Data will be getting this section 
            _result = await _jobService.GetJobByName(context.JobDetail.Key.Name.ToString());
            
            if (_result.IsSuccess)
            {
                _jobDataDto = _result.Data?.JobDataDtoList?.FirstOrDefault();
                if (_jobDataDto != null)
                {
                    switch (_jobDataDto.Method.ToLower())
                    {
                        case HttpMethodCodes.GET:
                            await _httpHelper.Get(_jobDataDto.Url, null, _jobDataDto.Body);
                            break;
                        case HttpMethodCodes.HEAD:
                            break;
                        case HttpMethodCodes.DELETE:
                            break;
                        case HttpMethodCodes.OPTIONS:
                            break;
                        case HttpMethodCodes.PATCH:
                            break;
                        case HttpMethodCodes.POST:
                            await _httpHelper.PostAsync(_jobDataDto.Url, null, _jobDataDto.Body);
                            break;
                        case HttpMethodCodes.PUT:
                            break;
                        case HttpMethodCodes.TRACE:
                            break;
                        default:
                            //Handle Invalid Method Code
                            break;
                    }
                }
            }
            GC.Collect();
        }
    }
}