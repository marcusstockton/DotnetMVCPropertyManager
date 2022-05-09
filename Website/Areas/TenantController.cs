using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ILogger<TenantController> _logger;
        private List<string> JobTitles = new List<string>(); // move this into a singleton service? 

        public TenantController(ILogger<TenantController> logger)
        {
            _logger = logger;
        }

        private async Task GetJobTitles()
        {
            var url = "https://raw.githubusercontent.com/jneidel/job-titles/master/job-titles.json";
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<JobTitleAutocompleteResponse>(data);
                JobTitles = result.JobTitles.ToList();
            }
        }

        // /api/Tenant/job-title-autocomplete?jobTitle="software"
        [HttpGet("job-title-autocomplete")] 
        public async Task<ActionResult> JobTitleAutoComplete(string jobTitle)
        {
            _logger.LogInformation($"{nameof(JobTitleAutoComplete)} finding job titles with {jobTitle}");

            if (!JobTitles.Any())
            {
                await GetJobTitles();
            }
            var jobTitles = JobTitles.Where(x => x.Contains(jobTitle)).ToList();
            return Ok(jobTitles);
        }
    }

    class JobTitleAutocompleteResponse
    {
        [JsonProperty("job-titles")]
        public string[] JobTitles { get; set; }
    }
}