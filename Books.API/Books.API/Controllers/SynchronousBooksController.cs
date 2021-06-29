using Books.API.Entities;
using Books.API.Filters;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Books.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SynchronousBooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public SynchronousBooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        [HttpGet]
        [BooksResultFilter]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            var bookEntities = _bookRepository.GetBooks();
            return Ok(bookEntities);
        }
    }
}
