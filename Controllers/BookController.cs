using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using trpo6_var2.Models;
using trpo6_var2.Service;

namespace trpo6_var2.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private BookService bookService = new BookService();

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? author,  [FromQuery] int? minYear)
        {
            var books = bookService.GetAll();
            if (!String.IsNullOrWhiteSpace(author))
            {
                if (bookService.GetAll().Count(x => x.Author == author) <= 0) return NotFound();
                books = books.Where(x => x.Author == author).ToList();
            }

            if (minYear != null)
            {
                if (minYear <= 0) return BadRequest();
                if (bookService.GetAll().Count(x => x.PublishYear >= minYear) <= 0) return NotFound();
                books = books.Where(x => x.PublishYear >= minYear).ToList();
            }

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            if (bookService.GetAll().Count(x => x.Id == id) > 0)
            {
                return Ok(bookService.GetAll().First(x => x.Id == id));
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Post(BookDTO bookDTO)
        {
            if (bookDTO == null ||
                String.IsNullOrWhiteSpace(bookDTO.Author) ||
                String.IsNullOrWhiteSpace(bookDTO.Title) ||
                bookDTO.PublishYear == null ||
                bookDTO.Price == null ||
                bookDTO.PublishYear <= 0 ||
                bookDTO.Price <= 0 ||
                bookDTO.IsAvailable == null)
                return BadRequest();

            Book book = new Book
            {
                Author = bookDTO.Author,
                Title = bookDTO.Title,
                PublishYear = (int)bookDTO.PublishYear,
                Price = (decimal)bookDTO.Price,
                IsAvailable = (bool)bookDTO.IsAvailable
            };

            bookService.Add(book);

            return Created();
        }
        
        [HttpPut("{id:int}")]
        public IActionResult Put([FromRoute]int id, BookDTO bookDTO)
        {
            if (bookService.GetAll().Count(b => b.Id == id) <= 0)
                return NotFound();

            if (bookDTO == null ||
                String.IsNullOrWhiteSpace(bookDTO.Author) ||
                String.IsNullOrWhiteSpace(bookDTO.Title) ||
                bookDTO.PublishYear == null ||
                bookDTO.Price == null ||
                bookDTO.PublishYear <= 0 ||
                bookDTO.Price <= 0 ||
                bookDTO.IsAvailable == null)
                return BadRequest();

            Book foundedBook = bookService.GetAll().First(b => b.Id == id);
            foundedBook.Author = bookDTO.Author;
            foundedBook.Title = bookDTO.Title;
            foundedBook.Price = (decimal)bookDTO.Price;
            foundedBook.PublishYear = (int)bookDTO.PublishYear;
            foundedBook.IsAvailable = (bool)bookDTO.IsAvailable;

            bookService.Commit();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch([FromRoute] int id, [FromBody] BookDTO bookDTO)
        {
            if (bookService.GetAll().Count(b => b.Id == id) <= 0)
                return NotFound();

            if (bookDTO == null)
                return BadRequest();

            bool allNull = true;

            Book foundedBook = bookService.GetAll().First(b => b.Id == id);
            if (bookDTO.Author != null)
            {
                allNull = false;
                if (String.IsNullOrWhiteSpace(bookDTO.Author))
                    return BadRequest();
                foundedBook.Author = bookDTO.Author;
            }
            if (bookDTO.Title != null)
            {
                allNull = false;
                if (String.IsNullOrWhiteSpace(bookDTO.Title))
                    return BadRequest();
                foundedBook.Title = bookDTO.Title;
            }
            if (bookDTO.Price != null)
            {
                allNull = false;
                if (bookDTO.Price <= 0)
                    return BadRequest();
                foundedBook.Price = (decimal)bookDTO.Price;
            }
            if (bookDTO.PublishYear != null)
            {
                allNull = false;
                if (bookDTO.PublishYear <= 0)
                    return BadRequest();
                foundedBook.PublishYear = (int)bookDTO.PublishYear;
            }
            if (bookDTO.IsAvailable != null)
            {
                allNull = false;
                foundedBook.IsAvailable = (bool)bookDTO.IsAvailable;
            }

            if (allNull)
                return BadRequest();

            bookService.Commit();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (bookService.GetAll().Count(b => b.Id == id) <= 0)
                return NotFound();
            
            bookService.Delete(bookService.GetAll().First(b => b.Id == id));
            
            return NoContent();
        }
    }
}
