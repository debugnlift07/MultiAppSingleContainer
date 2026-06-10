using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityNameController : ControllerBase
    {     

        [HttpGet(Name = "GetCityName")]
        public string GetCityName()
        {
            return "Noida";
        }
    }
}
