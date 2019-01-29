using AutoMapper;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Model;

namespace Piri.Framework.Scheduler.Quartz.Helper
{
    public static class MapperInitializer
    {
        public static void MapperConfiguration()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<JobDto, Job>()
                .ForMember(src => src.JobData, opt => opt.MapFrom(fa => fa.JobDataDtoList))
                .ReverseMap();

                config.CreateMap<Job, JobDto>()
               .ForMember(src => src.JobDataDtoList, opt => opt.MapFrom(fa => fa.JobData))
               .ReverseMap();

                config.CreateMap<JobDataDto, JobData>();

                config.CreateMap<JobData, JobDataDto>();

            });
        }
    }
}
