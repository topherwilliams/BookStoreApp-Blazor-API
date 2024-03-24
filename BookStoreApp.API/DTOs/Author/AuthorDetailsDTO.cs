using BookStoreApp.API.DTOs.Author;
using BookStoreApp.API.DTOs.Book;

namespace BookStoreApp.API.Models.Author
{
    public class AuthorDetailsDTO : AuthorReadOnlyDTO
    {
        public List<BookReadOnlyDTO> Books { get; set; }
    }
}
