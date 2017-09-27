using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pogodynka.Core.Domain;
using Pogodynka.Infrastructure.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pogodynka.Api.Controllers
{
    [Route("[controller]")]
    public class UploadController : Controller
    {
        private readonly IPhotoConditionsService _service;
        private readonly IHostingEnvironment _hostingEnvironment;
        public UploadController(IPhotoConditionsService service, IHostingEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;
        }       
       
        [HttpPost("")]//upload photo
        public async Task<IActionResult> Post(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    var filePath = _hostingEnvironment.WebRootPath + "/Uploads";
                    if (!Directory.Exists(filePath))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(filePath);
                    }
                    filePath=filePath+file.FileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        await stream.FlushAsync();
                    }                    

                    return Created($"{filePath}", new object());
                }
                return Content("Bad file");
            }
            catch (Exception exception)
            {
                return Content(exception.Message);
            }
        }

        [HttpPost("add")]// add conditions for this photo
        public async Task<IActionResult> Post([FromBody]Images conditions)
        {
            await _service.AddAsync(conditions);
            return Created($"with id {conditions.Id}", new object());
        }
    }
}
