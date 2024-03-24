using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Author;
using BookStoreApp.API.DTOs.Book;
using BookStoreApp.API.DTOs.User;
using BookStoreApp.API.Models.Author;

namespace BookStoreApp.API.Configurations
{
    public class MapperConfig : Profile
    {
           
        public MapperConfig() 
        {
            CreateMap<AuthorReadOnlyDTO, Author>().ReverseMap();
            CreateMap<AuthorDetailsDTO, Author>().ReverseMap();
            CreateMap<AuthorCreateDTO, Author>().ReverseMap();
            CreateMap<AuthorViewDTO, Author>().ReverseMap();
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();
            


            CreateMap<Book, BookReadOnlyDTO>()
                .ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
                .ReverseMap();

            //CreateMap<BookReadOnlyDTO, Book>().ReverseMap();

            CreateMap<BookCreateDTO, Book>().ReverseMap();
            CreateMap<BookUpdateDTO, Book>().ReverseMap();

            CreateMap<UserRegDTO, ApiUser>().ReverseMap();
        }

    }
}
