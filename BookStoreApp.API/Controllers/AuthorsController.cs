using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Author;
using AutoMapper;
using System.Net;
using BookStoreApp.API.Constants;
using Microsoft.AspNetCore.Authorization;
using BookStoreApp.API.Models.Author;
using AutoMapper.QueryableExtensions;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDTO>>> GetAuthors()
        {
            try
            {
                var authors = await _context.Authors.ToListAsync();
                var authorDTOs = mapper.Map<IEnumerable<AuthorReadOnlyDTO>>(authors);
                return Ok(authorDTOs);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GET - {nameof(GetAuthors)} - Error performing.");
                return StatusCode(500, StatusCodeMessages.Error500Message);
            }
            
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsDTO>> GetAuthor(int id)
        {
            try
            {
                var author = await _context.Authors
                    .Include(q => q.Books)
                    .Where(q => q.Id == id)
                    .ProjectTo<AuthorDetailsDTO>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();


                if (author == null)
                {
                    logger.LogWarning($"GET - {nameof(GetAuthor)} - ID {id} not found.");
                    return NotFound();
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GET - {nameof(GetAuthor)} - Error performing. ID: {id}.");
                return StatusCode(500, StatusCodeMessages.Error500Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDTO updatedAuthor)
        {
            if (id != updatedAuthor.Id) 
            {
                logger.LogWarning($"Update ID invalid in {nameof(PutAuthor)} - ID: {id}");
                return BadRequest();
            }

            var originalAuthor = await _context.Authors.FindAsync(id);
            
            if (originalAuthor == null)
            {
                logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} - ID: {id}");
                return NotFound();
            }

            mapper.Map(updatedAuthor, originalAuthor);  
            _context.Entry(originalAuthor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await AuthorExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    logger.LogError(ex, $"Error Performing GET in {nameof(PutAuthor)}");
                    return StatusCode(500, "500 Error");
                }
            }
            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Author>> PostAuthor(AuthorCreateDTO authorDTO)
        {
            var author = mapper.Map<Author>(authorDTO);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> AuthorExistsAsync(int id)
        {
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}
