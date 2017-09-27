using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Pogodynka.Core.Domain;
using Pogodynka.Infrastructure.Services;
using System.Threading.Tasks;

namespace Pogodynka.Api.Controllers
{
    [Route("[controller]")]
    public class ConditionsController:Controller
    {
        private readonly IPhotoConditionsService _service;        
        public ConditionsController(IPhotoConditionsService service)
        {
            _service = service;            
        }
        [HttpPost("")]// recieve photo for some conditions        
        public async Task<IActionResult> Post([FromBody]Images conditions)
        {
            var photo = await _service.RecieveAsync(conditions);
            return Json(photo);
        }
        [HttpGet("")]//get all
        public async Task<IActionResult> Get()
        {
            var photos = await _service.GetAllAsync();
            return Json(photos);
        }
    }
}