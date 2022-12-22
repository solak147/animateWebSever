using AnimateLibrary;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFileUpload.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public UploadController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        //[EnableCors("MyPolicy")]
        [HttpPost]
        public  IActionResult PostAsync(UploadedFile uploadedFile)
        {
            try
            {
                var path = $"{env.WebRootPath}\\{uploadedFile.FileName}";               
                var fs = System.IO.File.Create(path);
                fs.Write(uploadedFile.FileContent, 0, uploadedFile.FileContent.Length);
                fs.Close();

                return Ok();
            }
            catch
            {
                return BadRequest("Save File Failed");
            }
        }

    }
}
