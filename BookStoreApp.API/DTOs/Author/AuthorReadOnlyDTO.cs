using BookStoreApp.API.Models;

namespace BookStoreApp.API.DTOs.Author
{
    public class AuthorReadOnlyDTO : BaseDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Bio { get; set; }
    }
}
