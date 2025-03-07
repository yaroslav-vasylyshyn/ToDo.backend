using AutoMapper;
using ToDo.Infrastructure.DTO;

namespace  ToDo.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Task, CreateTaskDto>().ReverseMap();
        CreateMap<Task, UpdateTaskDto>().ReverseMap();
        CreateMap<Task, TaskDto>().ReverseMap();
    }
}
