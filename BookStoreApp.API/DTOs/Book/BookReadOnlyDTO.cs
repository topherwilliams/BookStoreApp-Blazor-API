using BookStoreApp.API.Models;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.DTOs.Book
{
    public class BookReadOnlyDTO: BaseDto
    {
        public string? Title { get; set; }

        public int Year { get; set; }

        public string Isbn { get; set; } = null!;

        public string? Summary { get; set; }

        public string? Image { get; set; }

        public decimal Price { get; set; }

        public int AuthorId { get; set; }

        public string? AuthorName { get; set; }

        //public virtual BookStoreApp.API.Data.Author? Author { get; set; }


    }
}
