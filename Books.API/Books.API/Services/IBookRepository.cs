using Books.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Services
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks();

        Book GetBook(Guid id);

        Task<IEnumerable<Book>> GetBooksAsync();

        Task<Book> GetBookAsync(Guid id);
    }
}
