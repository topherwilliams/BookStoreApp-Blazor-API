using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories
{
    public class AuthorRepository: GenericRepository<Author>, IAuthorRepository
    {
        private readonly BookStoreDbContext context;
        private readonly IMapper mapper;

        public AuthorRepository(BookStoreDbContext context, IMapper mapper): base(context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AuthorDetailsDTO> GetAuthorDetailsAsync(int id)
        {
            var author = await context.Authors
                    .Include(q => q.Books)
                    .Where(q => q.Id == id)
                    .ProjectTo<AuthorDetailsDTO>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            return author;
        }
    }
}
