using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services
{
    public class BookService : BaseHttpService, IBookService
    {
        private readonly IClient client;
        private readonly IMapper mapper;

        public BookService(IClient client, ILocalStorageService localStorage, IMapper mapper) : base(client, localStorage)
        {
            this.client = client;
            this.mapper = mapper;
        }

        public async Task<Response<int>> Create(BookCreateDTO book)
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await client.BooksPOSTAsync(book);
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<int>(exception);
            }

            return response;
        }

        public async Task<Response<int>> Delete(int id)
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await client.BooksDELETEAsync(id);
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<int>(exception);
            }

            return response;
        }

        public async Task<Response<int>> Edit(int id, BookUpdateDTO book)
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await client.BooksPUTAsync(id, book);
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<int>(exception);
            }

            return response;
        }

        public async Task<Response<List<BookReadOnlyDTO>>> Get()
        {
            Response<List<BookReadOnlyDTO>> response;

            try
            {
                await GetBearerToken();
                var data = await client.BooksAllAsync();
                response = new Response<List<BookReadOnlyDTO>>
                {
                    Data = data.ToList(),
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<List<BookReadOnlyDTO>>(exception);
            }

            return response;
        }

        public async Task<Response<BookDetailsDTO>> Get(int id)
        {
            Response<BookDetailsDTO> response;

            try
            {
                await GetBearerToken();
                var data = await client.BooksGETAsync(id);
                response = new Response<BookDetailsDTO>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<BookDetailsDTO>(exception);
            }

            return response;
        }

        public async Task<Response<BookUpdateDTO>> GetForUpdate(int id)
        {
            Response<BookUpdateDTO> response;

            try
            {
                await GetBearerToken();
                var data = await client.BooksGETAsync(id);
                response = new Response<BookUpdateDTO>
                {
                    Data = mapper.Map<BookUpdateDTO>(data),
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<BookUpdateDTO>(exception);
            }

            return response;
        }
    }
}
