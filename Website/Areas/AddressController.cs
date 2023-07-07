using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Website.Models.DTOs.Address;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private string _apiKey;
        private string _ukLatLong;
        private string _countryCode;
        private readonly ILogger<AddressController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public AddressController(ILogger<AddressController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _apiKey = Environment.GetEnvironmentVariable("HERE_Maps_API_Key", EnvironmentVariableTarget.User);
            _ukLatLong = "55.3781,3.4360"; // UK lat/lon
            _countryCode = "GBP";
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet, Route("getautosuggestion"), ResponseCache(CacheProfileName = "Default30")]
        public async Task<IActionResult> GetAutoSuggestion(string search)
        {
            _logger.LogInformation($"{nameof(GetAutoSuggestion)} Getting autosuggestions for {search}");
            using (HttpClient client = new HttpClient())
            {
                var url = $"https://autosuggest.search.hereapi.com/v1/autosuggest?at={_ukLatLong}&countryCode={_countryCode}&limit=50&lang=en&q={search}&apiKey={_apiKey}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(data);
                }
            }
            return BadRequest();
        }

        [HttpGet, Route("GetMapFromLatLong"), ResponseCache(VaryByQueryKeys = new string[] { "lat","lon" }, Duration = 1400)]
        public async Task<IActionResult> GetMapFromLatLong(decimal lat, decimal lon)
        {
            _logger.LogInformation($"{nameof(GetMapFromLatLong)} Getting Map for lat lon {lat} {lon}");
            using (HttpClient client = new HttpClient())
            {
                var url = $"https://image.maps.ls.hereapi.com/mia/1.6/mapview?apiKey={_apiKey}&c={lat},{lon}&vt=0&z=12";
                var response = await client.GetByteArrayAsync(url);
                return Ok("image/jpeg;base64," + Convert.ToBase64String(response));
            }
        }

        [HttpGet, Route("lookup")]
        public async Task<IActionResult> Lookup(string hereId)
        {
            _logger.LogInformation($"{nameof(Lookup)} Getting lookup data for {hereId}");
            using (HttpClient client = new HttpClient())
            {
                var url = $"https://lookup.search.hereapi.com/v1/lookup?id={hereId}&countryCode={_countryCode}&apiKey={_apiKey}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(await response.Content.ReadAsStringAsync());
                }
            }
            return BadRequest();
        }

        [HttpGet, Route("postcode-auto-complete")]
        public async Task<IActionResult> PostcodeAutoComplete(string postcode)
        {
            _logger.LogInformation($"{nameof(PostcodeAutoComplete)} querying postcode {postcode}");

            var client = _httpClientFactory.CreateClient("postcodesioClient");
            var url = $"{postcode}/autocomplete";
            var response = await client.GetFromJsonAsync<PostcodeAutoCompleteResult>(url);
            if (response.status == 200)
            {
                return Ok(response.result);
            }

            return Ok();
        }

        [HttpGet, Route("postcode-lookup")]
        public async Task<PostcodeLookup> PostCodeLookup(string postcode)
        {
            _logger.LogInformation($"{nameof(PostcodeAutoComplete)} querying postcode {postcode}");
            if (await VerifyPostcode(postcode))
            {
                var client = _httpClientFactory.CreateClient("postcodesioClient");
                var url = $"{postcode}";
                var response = await client.GetFromJsonAsync<PostcodeLookup>(url);
                if (response.status == 200)
                {
                    return response;
                }
                else
                {
                    throw new ArgumentException("Invalid Postcode");
                }
            }
            throw new ArgumentException("Invalid Postcode supplied");
        }

        private async Task<bool> VerifyPostcode(string postcode)
        {
            var client = _httpClientFactory.CreateClient("postcodesioClient");
            var url = $"{postcode}/validate";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadFromJsonAsync<VerifyPostcodeResult>();
                return jsonData.Result;
            }
            return false;
        }

        private class PostcodeAutoCompleteResult
        {
            public int status { get; set; }
            public string[] result { get; set; }
        }

        public class VerifyPostcodeResult
        {
            public int Status { get; set; }
            public bool Result { get; set; }
        }
    }
}