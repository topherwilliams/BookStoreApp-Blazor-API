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

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public BooksController(BookStoreDbContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDTO>>> GetBooks()
        {
            try
            {
                var bookDTOs = await _context.Books
                    .Include(x => x.Author)
                    .ProjectTo<BookReadOnlyDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();
                // *** ProjectTo casts as the object so no intermediate mapping needed. ***
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
        public async Task<ActionResult<BookReadOnlyDTO>> GetBook(int id)
        {
            try
            {
                var bookDTO = await _context.Books
                    .Include(x => x.Author)
                    .Where(q => q.Id == id)
                    .ProjectTo<BookReadOnlyDTO>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

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
        public async Task<IActionResult> PutBook(int id, BookUpdateDTO updatedBook)
        {
            if (id != updatedBook.Id)
            {
                // log
                return BadRequest();
            }

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                logger.LogWarning($"PUT - {nameof(PutBook)} - ID '{id}' not found.");
                return NotFound();
            }

            _context.Entry(mapper.Map<Author>(updatedBook)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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
        public async Task<ActionResult<Book>> PostBook(BookCreateDTO book)
        {
            try
            {
                Book newBook = mapper.Map<Book>(book);
                await _context.Books.AddAsync(newBook);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
            } catch (Exception ex)
            {
                logger.LogError(ex, $"POST - {nameof(PostBook)} - Error performing. Data: '{book.ToString}'.");
                return StatusCode(500, StatusCodeMessages.Error500Message);
            }

        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                logger.LogWarning($"DELETE - {nameof(PostBook)} - ID '{id}' not found.");
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
