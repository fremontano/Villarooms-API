using AutoMapper;
using villaroom_API.models;
using villaroom_API.models.Dto;

namespace villaroom_API
{
    public class MappingConfig : Profile
    {

        //Crear mapeo
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            //Otra forma de realizar 
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }
    }
}
