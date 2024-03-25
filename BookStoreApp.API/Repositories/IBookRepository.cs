using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.Book;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories
{
    public interface  IBookRepository: IGenericRepository<Book>
    {
        Task<BookDetailsDTO> GetBookDetailsAsync(int id);
        Task<List<BookReadOnlyDTO>> GetAllBookAsync();  
    }

    public class BookRepository: GenericRepository<Book>, IBookRepository
    {
        private readonly BookStoreDbContext context;
        private readonly IMapper mapper;

        public BookRepository(BookStoreDbContext context, IMapper mapper) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<BookReadOnlyDTO>> GetAllBookAsync()
        {
            var allBooks = await context.Books
                .Include(x => x.Author)
                .ProjectTo<BookReadOnlyDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
            // *** ProjectTo casts as the object so no intermediate mapping needed. ***
            return allBooks;
        }

        public async Task<BookDetailsDTO> GetBookDetailsAsync(int id)
        {
            var book = await context.Books
                .Include(x => x.Author)
                .Where(q => q.Id == id)
                .ProjectTo<BookDetailsDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return book;
        }
    }
}
