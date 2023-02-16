using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NLog.Targets;
using System.IO;

namespace RestaurantAPI.Controllers
{
    [Route("file")]
    /*[Authorize]*/
    public class FileControler : ControllerBase
    {
        [HttpGet ]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[]{"fileName"})]
        public IActionResult GetFile([FromQuery] string fileName)
        {
            var SolutionDirectory = Directory.GetCurrentDirectory();

            var FilePath = $"{SolutionDirectory}/PrivateFiles/{fileName}";

            var fileExist = System.IO.File.Exists(FilePath);

            if(!fileExist)
            {
                return NotFound();
            }
            var ContentProvider = new FileExtensionContentTypeProvider();

            ContentProvider.TryGetContentType(fileName, out var contentType);

            /*var contentType = Path.GetExtension(fileName);*/

            var FileBytes = System.IO.File.ReadAllBytes(FilePath);


            return File(FileBytes,contentType,fileName);
        }

        [HttpPost]
        public ActionResult Upload([FromForm]IFormFile file)
        {
            if(file != null && file.Length > 0)
            {
                var DirectoryPath = Directory.GetCurrentDirectory();

                var fullPath = $"{DirectoryPath}/PrivateFiles/{file.FileName}";

                using(var newStream = new FileStream(fullPath,FileMode.Create))
                { 
                    file.CopyTo(newStream);
                }
                return Ok();
            }
            return BadRequest();
        }
    }
}
