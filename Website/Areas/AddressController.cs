using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.Models.DTOs.Address;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public AddressController(IPropertyService propertyService, ILogger<AddressController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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
        public async Task<IActionResult> PostCodeLookup(string postcode)
        {
            _logger.LogInformation($"{nameof(PostcodeAutoComplete)} querying postcode {postcode}");
            if (await VerifyPostcode(postcode))
            {
                var client = _httpClientFactory.CreateClient("postcodesioClient");
                var url = $"{postcode}";
                var response = await client.GetFromJsonAsync<PostcodeLookup>(url);
                if (response.status == 200)
                {
                    return Ok(JsonSerializer.Serialize<PostcodeLookup>(response));
                }
                else
                {
                    return BadRequest("Invalid Postcode");
                }
            }
            return BadRequest("Invalid Postcode supplied");
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