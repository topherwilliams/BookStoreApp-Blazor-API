using System.ComponentModel.DataAnnotations;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.DTOs.Book
{
    public class BookUpdateDTO: BaseDto
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Title { get; set; }

        public int? Year { get; set; }

        [Required]
        [StringLength(50)]
        public string Isbn { get; set; } = null!;

        [StringLength(250)]
        public string? Summary { get; set; }

        [StringLength(50)]
        public string? Image { get; set; }


        public decimal? Price { get; set; }

    }
}
