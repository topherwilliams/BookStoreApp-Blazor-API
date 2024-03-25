using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;

namespace BookStoreApp.API.Repositories
{
    public interface IAuthorRepository: IGenericRepository<Author>
    {
        Task<AuthorDetailsDTO> GetAuthorDetailsAsync(int id);

    }
}
