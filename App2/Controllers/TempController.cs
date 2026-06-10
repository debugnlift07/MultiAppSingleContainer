using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TempController : ControllerBase
    {
       


        [HttpGet(Name = "GetCityTemp")]
        public string GetCityTemp()
        {
            return "48C";
        }
    }
}
