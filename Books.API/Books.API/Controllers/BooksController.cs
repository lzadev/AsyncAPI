using Books.API.Entities;
using Books.API.Filters;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        [HttpGet(Name = "GetBooks")]
        [BooksResultFilter]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var bookEntities = await _bookRepository.GetBooksAsync();
            return Ok(bookEntities);
        }

        [HttpGet("{id}",Name = "GetBook")]
        [BookResultFilter]
        public async Task<ActionResult<Book>> GetBook(Guid id)
        {
            var bookEntity = await _bookRepository.GetBookAsync(id);

            if(bookEntity == null)
            {
                return NotFound();
            }

            return Ok(bookEntity);
        }
    }
}
