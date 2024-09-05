using Microsoft.AspNetCore.Mvc;
using my_library_cosmos_db.Models;
using my_library_cosmos_db.Services;

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




    }
}
