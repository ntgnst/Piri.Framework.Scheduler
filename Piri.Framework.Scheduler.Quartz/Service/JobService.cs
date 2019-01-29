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
        private readonly QuartzDataContext _context;
        public JobService(QuartzDataContext context)
        {
            _context = context;
        }

        public async Task<Result<JobDto>> AddJob(JobDto jobDto)
        {
            Result<JobDto> result;
            Job job;
            try
            {
                //using (QuartzDataContext _context = new QuartzDataContext())
                //{

                job = Mapper.Map<JobDto, Job>(jobDto);
                // job.JobData.FirstOrDefault().Header = MapHeaderValue(jobDto.JobDataDtoList);
                await _context.Job.AddAsync(job);
                await _context.SaveChangesAsync();
                //}

                result = new Result<JobDto>(Mapper.Map<Job, JobDto>(job));
            }
            catch (Exception ex)
            {
                result = new Result<JobDto>(false, $"An error occured while adding job. Ex : {ex.ToString()}");
            }

            return result;
        }
        public async Task<Result<bool>> DeleteJob(int jobId)
        {
            Result<bool> result;
            Job job;
            try
            {
                //using (QuartzDataContext _context = new QuartzDataContext())
                //{
                job = await _context.Job.Where(w => w.Id.Equals(jobId)).FirstOrDefaultAsync();
                job.IsActive = false;
                _context.Job.Update(job);
                await _context.SaveChangesAsync();
                //}

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
                //using (QuartzDataContext _context = new QuartzDataContext())
                //{.Data.Where(w => w.IsActive && !w.IsRunning).ToList()
                jobList = await _context.Job.Include(i => i.JobData).Where(w => w.IsActive).ToListAsync();
                //}

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
            Result<JobDto> result;
            Job job;
            JobDto jobDto;
            try
            {
                //using (QuartzDataContext _context = new QuartzDataContext())
                //{
                job = await _context.Job.Where(w => w.Id.Equals(jobId)).FirstOrDefaultAsync();
                jobDto = Mapper.Map<Job, JobDto>(job);
                //}

                result = new Result<JobDto>(jobDto);
            }
            catch (Exception ex)
            {
                result = new Result<JobDto>(false, $"An error occured while getting job. Ex : {ex.ToString()}");
            }

            return result;
        }
        public async Task<Result<JobDto>> UpdateJob(JobDto jobDto)
        {
            Result<JobDto> result;
            Job job;
            try
            {
                //using (QuartzDataContext _context = new QuartzDataContext())
                //{
                job = Mapper.Map<JobDto, Job>(jobDto);
                _context.Job.Update(job);
                await _context.SaveChangesAsync();
                //}

                result = new Result<JobDto>(Mapper.Map<Job, JobDto>(job));
            }
            catch (Exception ex)
            {
                result = new Result<JobDto>(false, $"An error occured while updating job. Ex :{ex.ToString()}");
            }
            return result;
        }
        public async Task<Result<JobDto>> GetJobByName(string jobName)
        {
            _convertedName = jobName.Substring(0, 86);
            Result<JobDto> result;
            try
            {
                //using (QuartzDataContext _context = new QuartzDataContext())
                //{
                Job job = await _context.Job.Include(i => i.JobData).Where(w => w.JobData.FirstOrDefault().Name.Equals(_convertedName)).FirstOrDefaultAsync();
                result = new Result<JobDto>(Mapper.Map<Job, JobDto>(job));
                //}
            }
            catch (Exception ex)
            {
                result = new Result<JobDto>(false, $"An error occured while getting job by name. Ex: {ex.ToString()}");
            }
            return result;
        }
    }
}
