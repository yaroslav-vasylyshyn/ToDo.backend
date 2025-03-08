using AutoMapper;
using ToDo.Infrastructure.DTO;
using ToDo.Core.Models;

namespace  ToDo.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tasks, CreateTaskDto>().ReverseMap();
        CreateMap<Tasks, UpdateTaskDto>().ReverseMap();
        CreateMap<Tasks, TaskDto>().ReverseMap();
    }
}
