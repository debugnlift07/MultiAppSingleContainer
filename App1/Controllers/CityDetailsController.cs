using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityDetailsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CityDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("GetCityDetails")]
        public async Task<IActionResult> GetCityDetails()
        {
            var cityTask = _httpClient.GetStringAsync("http://localhost:8082/api/CityName");

            var tempTask = _httpClient.GetStringAsync("http://localhost:8081/api/Temp");

            await Task.WhenAll(cityTask, tempTask);

            var result = new
            {
                City = cityTask.Result,
                Temperature = tempTask.Result
            };

            return Ok(result);
        }
       
    }
}
