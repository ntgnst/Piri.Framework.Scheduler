using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Piri.Framework.Scheduler.Quartz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Service
{
    public class JobService : IJobService
    {
        private string _convertedName;
        private Job _job;
        private JobDto _jobDto;
        private Result<JobDto> _result;
        private Guid _guid;
        private readonly QuartzDataContext _context;
        public JobService(QuartzDataContext context)
        {
            _context = context;
        }
        public async Task<Result<JobDto>> AddJob(JobDto jobDto)
        {
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    _job = Mapper.Map<JobDto, Job>(jobDto);
                    _job.UpdatedDate = DateTime.Now;
                    _job.CreatedDate = DateTime.Now;
                    await _context.Job.AddAsync(_job);
                    await _context.SaveChangesAsync();
                    _result = new Result<JobDto>(Mapper.Map<Job, JobDto>(_job));
                    _context.Dispose();
                }
            }
            catch (Exception ex)
            {
                _result = new Result<JobDto>(false, $"An error occured while adding job. Ex : {ex.ToString()}");
            }

            return _result;
        }
        public async Task<Result<bool>> DeleteJob(int jobId)
        {
            Result<bool> result;
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    _job = await _context.Job.Where(w => w.Id.Equals(jobId)).FirstOrDefaultAsync();
                    _job.IsActive = false;
                    _context.Job.Update(_job);
                    await _context.SaveChangesAsync();
                    result = new Result<bool>(true, "Job was successfully deleted.");
                    _context.Dispose();
                }
            }
            catch (Exception ex)
            {
                result = new Result<bool>(false, $"An error occured while deleting job. Ex :{ex.ToString()}");
            }
            return result;
        }
        public async Task<Result<List<JobDto>>> GetAllJobs()
        {
            Result<List<JobDto>> result;
            List<Job> jobList;
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    jobList = await _context.Job.Include(i => i.JobData).Where(w => w.IsActive).ToListAsync();
                    result = new Result<List<JobDto>>(Mapper.Map<List<Job>, List<JobDto>>(jobList));
                    _context.Dispose();
                }
            }
            catch (Exception ex)
            {
                result = new Result<List<JobDto>>(false, $"An error occured while getting all jobs . Ex {ex.ToString()}");
            }
            return result;
        }
        public async Task<Result<JobDto>> GetJobById(int jobId)
        {
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    _job = await _context.Job.Where(w => w.Id.Equals(jobId)).FirstOrDefaultAsync();
                    _jobDto = Mapper.Map<Job, JobDto>(_job);
                    _result = new Result<JobDto>(_jobDto);
                    _context.Dispose();
                }
            }
            catch (Exception ex)
            {
                _result = new Result<JobDto>(false, $"An error occured while getting job. Ex : {ex.ToString()}");
            }

            return _result;
        }
        public async Task<Result<JobDto>> UpdateJob(JobDto jobDto)
        {
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    _job = Mapper.Map<JobDto, Job>(jobDto);
                    _job.UpdatedDate = DateTime.Now;
                    _context.Job.Update(_job);
                    await _context.SaveChangesAsync();
                    _result = new Result<JobDto>(Mapper.Map<Job, JobDto>(_job));
                    _context.Dispose();
                }
            }
            catch (Exception ex)
            {
                _result = new Result<JobDto>(false, $"An error occured while updating job. Ex :{ex.ToString()}");
            }
            return _result;
        }
        public async Task<Result<JobDto>> GetJobByName(string jobName)
        {
            _convertedName = jobName.Substring(0, 86);
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    _job = await _context.Job.Include(i => i.JobData).Where(w => w.JobData.FirstOrDefault().Name.Equals(_convertedName)).FirstOrDefaultAsync();
                    _result = new Result<JobDto>(Mapper.Map<Job, JobDto>(_job));
                    _context.Dispose();
                }
            }
            catch (Exception ex)
            {
                _result = new Result<JobDto>(false, $"An error occured while getting job by name. Ex: {ex.ToString()}");
            }
            return _result;
        }
        public async Task<Result<string>> DeleteJob(string guid)
        {
            Result<string> result;
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    _guid = Guid.Parse(guid);
                    Job job = await _context.Job.Where(w => w.Guid.Equals(_guid)).FirstOrDefaultAsync();
                    _context.JobData.RemoveRange(job.JobData);
                    _context.Job.Remove(job);
                    await _context.SaveChangesAsync();
                }
                result = new Result<string>(true, ResultTypeEnum.Success, $"Succesfully deleted Job GUID : {guid.ToUpperInvariant()}");
            }
            catch (Exception ex)
            {
                result = new Result<string>(true, ResultTypeEnum.Success, $"Deletion of Job unsuccessful. Ex : {ex.ToString()}");
            }

            return result;
        }

        public async Task<Result<List<Result<JobDto>>>> PauseAll()
        {
            Result<List<Result<JobDto>>> result;
            List<Result<JobDto>> innerResult = new List<Result<JobDto>>();
            try
            {
                Result<List<JobDto>> allJobs = await GetAllJobs();
                
                    allJobs.Data?.ForEach(async f =>
                    {
                        f.IsPaused = true;
                        innerResult.Add(await UpdateJob(f));
                    });

                result = new Result<List<Result<JobDto>>>(innerResult);
            }
            catch (Exception ex)
            {
                result = new Result<List<Result<JobDto>>>(false, $"An error occured while pausing all jobs. Ex : {ex.ToString()}");
            }

            return result;
        }
    }
}
