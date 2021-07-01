using AutoMapper;
using Books.API.Exceptions;
using Books.API.Filters;
using Books.API.Models;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetBooks")]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            throw new LogicException("Hola");
            var bookEntities = await _bookRepository.GetBooksAsync();
            return Ok(bookEntities);
        }

        [HttpGet("{id}", Name = "GetBook")]
        [BookResultFilter]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var bookEntity = await _bookRepository.GetBookAsync(id);

            if (bookEntity == null)
            {
                return NotFound();
            }

            return Ok(bookEntity);
        }

        [HttpPost]
        [BookResultFilter]
        public async Task<IActionResult> CreateBook(BookForCreation bookForCreation)
        {
            var bookToAdd = _mapper.Map<Entities.Book>(bookForCreation);

            _bookRepository.AddBook(bookToAdd);

            //para cargar el author en el context y no me retorne un author null
            await _bookRepository.GetBookAsync(bookToAdd.Id);

            await _bookRepository.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetBook), new { id = bookToAdd.Id }, bookToAdd);

        }
    }
}
