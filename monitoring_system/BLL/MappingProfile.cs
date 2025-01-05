using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Мапінг сутності <-> DTO
            CreateMap<Sensor, SensorDTO>().ReverseMap();
            CreateMap<Scientist, ScientistDTO>().ReverseMap();
            CreateMap<Data, DataDTO>().ReverseMap();

        }
    }
}
