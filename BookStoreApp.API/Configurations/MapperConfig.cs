using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Author;
using BookStoreApp.API.DTOs.Book;

namespace BookStoreApp.API.Configurations
{
    public class MapperConfig : Profile
    {
           
        public MapperConfig() 
        { 
            CreateMap<AuthorCreateDTO, Author>().ReverseMap();
            CreateMap<AuthorViewDTO, Author>().ReverseMap();
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();

            CreateMap<Book, BookReadOnlyDTO>()
                .ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
                .ReverseMap();

            //CreateMap<BookReadOnlyDTO, Book>().ReverseMap();

            CreateMap<BookCreateDTO, Book>().ReverseMap();
            CreateMap<BookUpdateDTO, Book>().ReverseMap();
        }

    }
}
