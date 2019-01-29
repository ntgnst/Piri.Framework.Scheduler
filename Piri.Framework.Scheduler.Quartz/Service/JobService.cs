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
        private readonly QuartzDataContext _context;
        public JobService(QuartzDataContext context)
        {
            _context = context;
        }
        public async Task<Result<JobDto>> AddJob(JobDto jobDto)
        {
            try
            {
                _job = Mapper.Map<JobDto, Job>(jobDto);
                await _context.Job.AddAsync(_job);
                await _context.SaveChangesAsync();
                _result = new Result<JobDto>(Mapper.Map<Job, JobDto>(_job));
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
                _job = await _context.Job.Where(w => w.Id.Equals(jobId)).FirstOrDefaultAsync();
                _job.IsActive = false;
                _context.Job.Update(_job);
                await _context.SaveChangesAsync();
                result = new Result<bool>(true, "Job was successfully deleted.");
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
                jobList = await _context.Job.Include(i => i.JobData).Where(w => w.IsActive).ToListAsync();
                result = new Result<List<JobDto>>(Mapper.Map<List<Job>, List<JobDto>>(jobList));
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
                _job = await _context.Job.Where(w => w.Id.Equals(jobId)).FirstOrDefaultAsync();
                _jobDto = Mapper.Map<Job, JobDto>(_job);
                _result = new Result<JobDto>(_jobDto);
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
                _job = Mapper.Map<JobDto, Job>(jobDto);
                _context.Job.Update(_job);
                await _context.SaveChangesAsync();
                _result = new Result<JobDto>(Mapper.Map<Job, JobDto>(_job));
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
                _job = await _context.Job.Include(i => i.JobData).Where(w => w.JobData.FirstOrDefault().Name.Equals(_convertedName)).FirstOrDefaultAsync();
                _result = new Result<JobDto>(Mapper.Map<Job, JobDto>(_job));
            }
            catch (Exception ex)
            {
                _result = new Result<JobDto>(false, $"An error occured while getting job by name. Ex: {ex.ToString()}");
            }
            return _result;
        }
    }
}
