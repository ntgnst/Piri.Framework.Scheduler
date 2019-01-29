using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Piri.Framework.Scheduler.Quartz.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Piri.Framework.Scheduler.Quartz.Service
{
    public class JobService : IJobService
    {
        public JobService()
        {
        }

        public Result<JobDto> AddJob(JobDto jobDto)
        {
            Result<JobDto> result;
            Job job;
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {

                    job = Mapper.Map<JobDto, Job>(jobDto);
                   // job.JobData.FirstOrDefault().Header = MapHeaderValue(jobDto.JobDataDtoList);
                    _context.Job.Add(job);
                    _context.SaveChanges();
                }

                result = new Result<JobDto>(Mapper.Map<Job, JobDto>(job));
            }
            catch (Exception ex)
            {
                result = new Result<JobDto>(false, $"An error occured while adding job. Ex : {ex.ToString()}");
            }

            return result;
        }
        public Result<bool> DeleteJob(int jobId)
        {
            Result<bool> result;
            Job job;
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    job = _context.Job.Where(w => w.Id.Equals(jobId)).FirstOrDefault();
                    job.IsActive = false;
                    _context.Job.Update(job);
                    _context.SaveChanges();
                }

                result = new Result<bool>(true, "Job was successfully deleted.");
            }
            catch (Exception ex)
            {
                result = new Result<bool>(false, $"An error occured while deleting job. Ex :{ex.ToString()}");
            }
            return result;
        }
        public Result<List<JobDto>> GetAllJobs()
        {
            Result<List<JobDto>> result;
            List<Job> jobList;
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    jobList = _context.Job.Include(i => i.JobData).ToList();
                }

                result = new Result<List<JobDto>>(Mapper.Map<List<Job>, List<JobDto>>(jobList));
            }
            catch (Exception ex)
            {
                result = new Result<List<JobDto>>(false, $"An error occured while getting all jobs . Ex {ex.ToString()}");
            }
            return result;
        }
        public Result<JobDto> GetJobById(int jobId)
        {
            Result<JobDto> result;
            Job job;
            JobDto jobDto;
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    job = _context.Job.Where(w => w.Id.Equals(jobId)).FirstOrDefault();
                    jobDto = Mapper.Map<Job, JobDto>(job);
                }

                result = new Result<JobDto>(jobDto);
            }
            catch (Exception ex)
            {
                result = new Result<JobDto>(false, $"An error occured while getting job. Ex : {ex.ToString()}");
            }

            return result;
        }
        public Result<JobDto> UpdateJob(JobDto jobDto)
        {
            Result<JobDto> result;
            Job job;
            try
            {
                using (QuartzDataContext _context = new QuartzDataContext())
                {
                    job = Mapper.Map<JobDto, Job>(jobDto);
                    _context.Job.Update(job);
                    _context.SaveChanges();
                }

                result = new Result<JobDto>(Mapper.Map<Job, JobDto>(job));
            }
            catch (Exception ex)
            {
                result = new Result<JobDto>(false, $"An error occured while updating job. Ex :{ex.ToString()}");
            }
            return result;
        }
    }
}
