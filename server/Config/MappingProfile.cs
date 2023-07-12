using AutoMapper;
using BusinessObject;
using server.Dto.Job;
using server.Dto.User;

namespace server.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<User, UserResponse>()
                .ForMember(dto => dto.Role, 
                opt => opt.MapFrom(user => user.UserRoles != null ? user.UserRoles.ToList().Select(ur => ur.Role.Name).ToList() : null));

            CreateMap<JobCreateRequest, Job>();
        }
    }
}
