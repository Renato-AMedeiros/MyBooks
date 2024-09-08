using Microsoft.AspNetCore.Mvc;
using my_library_cosmos_db.Models;
using my_library_cosmos_db.Services;
using MyBooks.Core.Models;

namespace my_library_cosmos_db.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class LibrariesController : Controller
    {

        private readonly LibrariesService _librariesService;

        public LibrariesController(LibrariesService librariesService)
        {
            _librariesService = librariesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequestModel model)
        {
            var response = await _librariesService.CreateBook(model);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookRequestModel model, Guid bookId)
        {
            var response = await _librariesService.UpdateBook(model, bookId);

            return Ok(response);
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetBookById([FromQuery] Guid bookId, [FromQuery] string partitionKey)
        {
            var response = await _librariesService.GetBookById(bookId, partitionKey);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookList()
        {
            var response = await _librariesService.GetBookList();

            return Ok(response);
        }

    }
}
