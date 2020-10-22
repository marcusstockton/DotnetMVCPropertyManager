using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private string _apiKey;
        private string ukLatLong;
        public AddressController()
        {
            _apiKey = Environment.GetEnvironmentVariable("HERE_Maps_API_Key", EnvironmentVariableTarget.User);
            ukLatLong = "55.3781,3.4360";
        }

        [HttpGet, Route("GetAutoSuggestion")]
        public async Task<IActionResult> GetAutoSuggestion(string search)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = $"https://autosuggest.search.hereapi.com/v1/autosuggest?at={ukLatLong}&limit=5&lang=en&q={search}&apiKey={_apiKey}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(data);
                }
            }
            return BadRequest();
        }

        [HttpGet, Route("Lookup")]
        public async Task<IActionResult> Lookup(string hereId)
        {
            using (HttpClient client = new HttpClient())
            {
                //   var url = $"https://autosuggest.search.hereapi.com/v1/autosuggest?at={ukLatLong}&limit=5&lang=en&q={search}&apiKey={_apiKey}";
                var url = $"https://lookup.search.hereapi.com/v1/lookup?id={hereId}&apiKey={_apiKey}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(data);
                }
            }
            return BadRequest();
        
        }
    }
}
