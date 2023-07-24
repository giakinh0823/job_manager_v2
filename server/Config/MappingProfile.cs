using AutoMapper;
using server.Dto.Job;
using server.Dto.Log;
using server.Dto.User;
using server.Entity;

namespace server.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<User, UserResponse>()
                .ForMember(dto => dto.Role, 
                opt => opt.MapFrom(user => user.UserRoles != null ? user.UserRoles.ToList().Select(ur => ur.Role.Name).ToList() : null));

            CreateMap<JobCreateRequest, Job>();
            CreateMap<Log, LogResponse>();
            CreateMap<Job, JobResponse>().ForMember(dest => dest.Logs, src => src.MapFrom(job => job.Logs.Select(log => new LogResponse
            {
                LogId = log.LogId,
                JobId = log.JobId,
                UserId = log.UserId,
                StartTime = log.StartTime,
                EndTime = log.EndTime,
                Status = log.Status,
                Output = log.Output
            })));
        }
    }
}
