using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        [HttpGet("job-title-autocomplete")]
        public async Task<ActionResult> JobTitleAutoComplete(string jobTitle)
        {
            var url = $"http://api.dataatwork.org/v1/jobs/autocomplete?begins_with={jobTitle}";
            HttpClient req = new HttpClient();
            var content = await req.GetAsync(url);
            if (content.IsSuccessStatusCode)
            {
                return Ok(content.Content.ReadAsStringAsync().Result);
            }
            return BadRequest(content);
        }
    }
}