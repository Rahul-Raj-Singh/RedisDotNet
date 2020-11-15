using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisDemo.Services;

namespace RedisDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ForecastService _forecastService;

        public WeatherForecastController(ForecastService forecastService)
        {
            _forecastService = forecastService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _forecastService.GetForecastsAsync();

            return Ok(new { From = result.Item1, Data = result.Item2});

        }
    }
}
