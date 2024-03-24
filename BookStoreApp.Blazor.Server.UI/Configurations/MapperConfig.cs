using AutoMapper;
using BookStoreApp.Blazor.Server.UI.Services.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStoreApp.Blazor.Server.UI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorReadOnlyDTO, AuthorUpdateDTO>().ReverseMap();
            CreateMap<AuthorDetailsDTO, AuthorUpdateDTO>().ReverseMap();
            CreateMap<BookReadOnlyDTO, BookUpdateDTO>().ReverseMap();
            CreateMap<BookDetailsDTO, BookUpdateDTO>().ReverseMap();

        }
    }
}
