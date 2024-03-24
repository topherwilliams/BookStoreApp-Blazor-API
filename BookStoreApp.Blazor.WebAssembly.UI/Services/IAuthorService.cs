using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services
{
    public interface IAuthorService
    {
        Task<Response<List<AuthorReadOnlyDTO>>> Get();
        Task<Response<AuthorDetailsDTO>> Get(int id);
        Task<Response<AuthorUpdateDTO>> GetForUpdate(int id);
        Task<Response<int>> Create(AuthorCreateDTO author);
        Task<Response<int>> Edit(int id, AuthorUpdateDTO author);
        Task<Response<int>> Delete(int id);
    }
}
