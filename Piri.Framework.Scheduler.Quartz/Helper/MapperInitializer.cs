using AutoMapper;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Model;
using System.Collections.Generic;

namespace Piri.Framework.Scheduler.Quartz.Helper
{
    public static class MapperInitializer
    {
        public static void MapperConfiguration()
        {
            string builder = string.Empty;
            List<KeyValuePair<string, string>> keyValuePairList;

            Mapper.Initialize(config =>
            {
                config.CreateMap<JobDto, Job>()
                .ForMember(src => src.JobData, opt => opt.MapFrom(fa => fa.JobDataDtoList))
                .ReverseMap();

                config.CreateMap<Job, JobDto>()
               .ForMember(src => src.JobDataDtoList, opt => opt.MapFrom(fa => fa.JobData))
               .ReverseMap();

                config.CreateMap<JobDataDto, JobData>()
                //.BeforeMap((src, dest) =>
                //{
                //    src.Header.ForEach(f =>
                //    {
                //        builder += $"{f.Key.ToString()},{f.Value.ToString()};";
                //    });
                //    dest.Header = builder;
                //})
                ;

                config.CreateMap<JobData, JobDataDto>()
                //.BeforeMap((src, dest) =>
                //{
                //    keyValuePairList = new List<KeyValuePair<string, string>>();
                //    string[] keyValueArray = src.Header.Split(";");
                //    string[] temp;
                //    for (int i = 0; i < keyValueArray.Length; i++)
                //    {
                //        temp = keyValueArray[i].Split(',');
                //        keyValuePairList.Add(new KeyValuePair<string, string>(temp[0], temp[1]));
                //    }
                //    dest.Header = keyValuePairList;
                //})
                ;

            });
        }
    }
}
