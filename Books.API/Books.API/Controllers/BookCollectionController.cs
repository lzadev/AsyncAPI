using AutoMapper;
using Books.API.Filters;
using Books.API.ModelBinders;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BooksResultFilter]
    public class BookCollectionController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookCollectionController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({bookIds})", Name = "GetBookCollection")]

        public async Task<IActionResult> GetBookCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinders))] IEnumerable<Guid> bookIds)
        {
            var bookEntities = await _bookRepository.GetBooksAsync(bookIds);

            if (bookIds.Count() != bookIds.Count())
            {
                return NotFound();
            }

            return Ok(bookEntities);

        }

        [HttpPost]
        public async Task<IActionResult> CreateBookCollection(IEnumerable<Models.Book> bookCollection)
        {
            var bookEntities = _mapper.Map<IEnumerable<Entities.Book>>(bookCollection);

            foreach (var entityBook in bookEntities)
            {
                _bookRepository.AddBook(entityBook);
            }

            await _bookRepository.SaveChangesAsync();

            var bookToReturn = await _bookRepository.GetBooksAsync(bookEntities.Select(b => b.Id).ToList());

            var bookIds = string.Join(",", bookToReturn.Select(b => b.Id));

            return CreatedAtRoute(nameof(GetBookCollection),
                                    new { bookIds = bookIds }, bookToReturn);
        }
    }
}
