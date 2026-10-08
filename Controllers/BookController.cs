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
            if (book == null)
                return BadRequest();
            bookService.Add(book);

            return Ok();
        }
    }
}
