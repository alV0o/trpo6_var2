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
        public IActionResult GetAll()
        {
            return Ok(bookService.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            if (bookService.GetAll().Where(x => x.Id == id).Count() > 0)
            {
                return Ok(bookService.GetAll().Where(x => x.Id == id).First());
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Post(Book book)
        {
            if (book == null ||
                String.IsNullOrEmpty(book.Author) ||
                String.IsNullOrEmpty(book.Title) ||
                book.PublishYear <= 0 ||
                book.Price <= 0)
                return BadRequest();

            book.Id = 0;
            
            bookService.Add(book);

            return Ok();
        }
        
        [HttpPut("{id:int}")]
        public IActionResult Put([FromRoute]int id, Book book)
        {
            if (bookService.GetAll().Where(b => b.Id == id).Count() <= 0)
                return NotFound();

            if (book == null ||
                String.IsNullOrEmpty(book.Author) ||
                String.IsNullOrEmpty(book.Title) ||
                book.PublishYear <= 0 ||
                book.Price <= 0)
                return BadRequest();

            Book foundedBook = bookService.GetAll().Where(b => b.Id == id).First();
            foundedBook.Author = book.Author;
            foundedBook.Title = book.Title;
            foundedBook.Price = book.Price;
            foundedBook.PublishYear = book.PublishYear;
            foundedBook.IsAvailable = book.IsAvailable;

            bookService.Commit();

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch([FromBody]string json)
        {
            return Ok();
        }
    }
}
