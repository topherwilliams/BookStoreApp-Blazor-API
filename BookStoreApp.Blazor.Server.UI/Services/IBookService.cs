using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services
{
    public interface IBookService
    {
        Task<Response<List<BookReadOnlyDTO>>> Get();
        Task<Response<BookDetailsDTO>> Get(int id);
        Task<Response<BookUpdateDTO>> GetForUpdate(int id);
        Task<Response<int>> Create(BookCreateDTO book);
        Task<Response<int>> Edit(int id, BookUpdateDTO book);
        Task<Response<int>> Delete(int id);
    }
}
