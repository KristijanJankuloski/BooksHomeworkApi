using BooksHomeworkApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksHomeworkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            return Ok(StaticDb.Books);
        }

        [HttpGet("search")]
        public ActionResult<Book> GetByIndex(int index)
        {
            try
            {
                if (index < 0)
                    return BadRequest("Index cannot be negative");
                if (index >= StaticDb.Books.Count)
                    NotFound($"Index {index} not found");
                return Ok(StaticDb.Books[index]);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        [HttpGet("filter")]
        public ActionResult<Book> GetByAuthor(string? author, string? title)
        {
            try
            {
                if (string.IsNullOrEmpty(author) && string.IsNullOrEmpty(title))
                    return BadRequest("At least one parameter must be filled");

                if(!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(title))
                {
                    Book book = StaticDb.Books.FirstOrDefault(x => x.Author.ToLower() == author.ToLower() && x.Title.ToLower() == title.ToLower());
                    if (book == null)
                        return NotFound("No book found");
                    return Ok(book);
                }

                if (string.IsNullOrEmpty(title))
                {
                    Book book = StaticDb.Books.FirstOrDefault(x => x.Author.ToLower() == author.ToLower());
                    if (book == null)
                        return NotFound("No book found");
                    return Ok(book);
                }

                Book book1 = StaticDb.Books.FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
                if (book1 == null)
                    return NotFound("No book found");
                return Ok(book1);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest("No book provided");

            if (string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author))
                return BadRequest("Title and author fields must be full");

            StaticDb.Books.Add(book);
            return StatusCode(StatusCodes.Status201Created, "Book added");
        }

        [HttpPost("titles")]
        public ActionResult<List<string>> ExtractTitles([FromBody] List<Book> books)
        {
            if (books == null)
                return BadRequest("No books provided");
            if (books.Count == 0)
                return BadRequest("Book array is empty");

            return Ok(books.Select(x => x.Title).ToList());
        }
    }
}
