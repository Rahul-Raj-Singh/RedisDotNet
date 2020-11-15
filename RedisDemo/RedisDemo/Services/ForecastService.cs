using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedisDemo.Extensions;

namespace RedisDemo.Services
{
    public class ForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IDistributedCache _cache;

        public ForecastService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<Tuple<string, WeatherForecast[]>> GetForecastsAsync()
        {
            var key = DateTime.Now.ToString("ddMMyyyy-hhmm");
            var value = await _cache.GetRecordAsync<WeatherForecast[]>(key);
            if(value is null)
            {
                // Get records from Db
                await Task.Delay(5000);
                var random = new Random();
                var records = Enumerable.Range(1, 5).Select(i => new WeatherForecast
                {
                    Date = DateTime.Now,
                    TemperatureC = random.Next(-20, 55),
                    Summary = Summaries[random.Next(Summaries.Length)]
                }).ToArray();

                // Cache in Redis
                await _cache.SetRecordAsync<WeatherForecast[]>(key, records);

                return Tuple.Create($"DB: {key}", records);

            }

            //From Redis Cache
            return Tuple.Create($"Redis: {key}", value);

        }
    }
}
