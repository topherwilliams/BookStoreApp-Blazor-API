﻿using BookStoreApp.API.Data;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.DTOs.Author
{
    public class AuthorViewDTO
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Bio {  get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();


    }
}
