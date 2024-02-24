using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Author;

namespace BookStoreApp.API.Configurations
{
    public class MapperConfig : Profile
    {
           
        public MapperConfig() 
        { 
            CreateMap<AuthorCreateDTO, Author>().ReverseMap();
            CreateMap<AuthorViewDTO, Author>().ReverseMap();
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();
        }

    }
}
