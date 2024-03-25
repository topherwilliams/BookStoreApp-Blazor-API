using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Book;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Constants;
using Microsoft.AspNetCore.Authorization;
using BookStoreApp.API.Repositories;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public BooksController(IBookRepository bookRepository, IMapper mapper, ILogger<BooksController> logger)
        {
            this.bookRepository = bookRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDTO>>> GetBooks()
        {
            try
            {
                var bookDTOs = await bookRepository.GetAllBookAsync();
                return Ok(bookDTOs);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GET - {nameof(GetBooks)} - Error performing.");
                return StatusCode(500, StatusCodeMessages.Error500Message);
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDTO>> GetBook(int id)
        {
            try
            {
                var bookDTO = await bookRepository.GetBookDetailsAsync(id);

                if (bookDTO == null) 
                {
                    logger.LogWarning($"GET - {nameof(GetBook)} - ID '{id}' not found.");
                    return NotFound();
                }

                return Ok(bookDTO);
            } catch (Exception ex)
            {
                logger.LogError(ex, $"GET - {nameof(GetBook)} - Error performing. ID '{id}'.");
                return StatusCode(500, StatusCodeMessages.Error500Message);
            }
            
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDTO updatedBook)
        {
            if (id != updatedBook.Id)
            {
                // log
                return BadRequest();
            }

            var existingBook = await bookRepository.GetAsync(id);
            if (existingBook == null)
            {
                logger.LogWarning($"PUT - {nameof(PutBook)} - ID '{id}' not found.");
                return NotFound();
            }
            
            mapper.Map(updatedBook, existingBook);

            try
            {
                await bookRepository.UpdateAsync(existingBook);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Book>> PostBook(BookCreateDTO book)
        {
            try
            {
                Book newBook = mapper.Map<Book>(book);
                await bookRepository.AddAsync(newBook);

                return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
            } catch (Exception ex)
            {
                logger.LogError(ex, $"POST - {nameof(PostBook)} - Error performing. Data: '{book.ToString}'.");
                return StatusCode(500, StatusCodeMessages.Error500Message);
            }

        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await bookRepository.GetAsync(id);

            if (book == null)
            {
                logger.LogWarning($"DELETE - {nameof(PostBook)} - ID '{id}' not found.");
                return NotFound();
            }

            await bookRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> BookExists(int id)
        {
            return await bookRepository.Exists(id);
        }
    }
}
