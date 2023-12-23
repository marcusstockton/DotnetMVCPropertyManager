using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Website.Interfaces;
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
        private readonly IPropertyService _propertyService;
        private readonly ILogger<AddressController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public AddressController(IPropertyService propertyService, ILogger<AddressController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _apiKey = Environment.GetEnvironmentVariable("HERE_Maps_API_Key", EnvironmentVariableTarget.User);
            _ukLatLong = "55.3781,3.4360"; // UK lat/lon
            _countryCode = "GBP";
            _propertyService = propertyService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet, Route("getautosuggestion"), ResponseCache(CacheProfileName = "Default30")]
        public async Task<IActionResult> GetAutoSuggestion(string search)
        {
            _logger.LogInformation($"{nameof(GetAutoSuggestion)} Getting autosuggestions for {search}");

            var client = _httpClientFactory.CreateClient("hereApiAutosuggest");
            var url = $"autosuggest?at={_ukLatLong}&countryCode={_countryCode}&limit=50&lang=en&q={search}&apiKey={_apiKey}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest();
        }

        [HttpGet, Route("GetMapFromLatLong"), ResponseCache(VaryByQueryKeys = new string[] { "portfolioId", "propertyId", "lat", "lon" }, Duration = 1400)]
        public async Task<IActionResult> GetMapFromLatLong(string portfolioId, string propertyId, double lat, double lon)
        {
            _logger.LogInformation($"{nameof(GetMapFromLatLong)} Getting Map for lat lon {lat} {lon}");
            if (Guid.Parse(portfolioId) == Guid.Empty || Guid.Parse(propertyId) == Guid.Empty)
            {
                return BadRequest("Please pass in a portfolio id and property id");
            }

            var property = await _propertyService.GetPropertyById(Guid.Parse(portfolioId), Guid.Parse(propertyId));
            if (property != null)
            {
                if (property.MapImage != null && property.Address.Latitude == lat && property.Address.Longitude == lon)
                {
                    return Ok("image/jpeg;base64," + Convert.ToBase64String(property.MapImage));
                }
                else
                {
                    var client = _httpClientFactory.CreateClient("hereApiImages");
                    var url = $"?apiKey={_apiKey}&c={lat},{lon}&vt=0&z=17&t=9";
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("image/jpeg;base64," + Convert.ToBase64String(await response.Content.ReadAsByteArrayAsync()));
                    }
                }
            }
            return BadRequest("Unable to find the property with the supplied data.");
        }

        [HttpGet, Route("lookup")]
        public async Task<IActionResult> Lookup(string hereId)
        {
            _logger.LogInformation($"{nameof(Lookup)} Getting lookup data for {hereId}");
            var client = _httpClientFactory.CreateClient("hereApiLookup");
            var url = $"lookup?id={hereId}&countryCode={_countryCode}&apiKey={_apiKey}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
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