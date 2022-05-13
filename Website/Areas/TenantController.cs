using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Website.Services;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ILogger<TenantController> _logger;
        private readonly IJobTitleService _jobTitleService;

        public TenantController(ILogger<TenantController> logger, IJobTitleService jobTitleService)
        {
            _logger = logger;
            _jobTitleService = jobTitleService;
        }


        // /api/Tenant/job-title-autocomplete?jobTitle="software"
        [HttpGet("job-title-autocomplete")] 
        public async Task<ActionResult> JobTitleAutoComplete(string jobTitle)
        {
            _logger.LogInformation($"{nameof(JobTitleAutoComplete)} finding job titles with {jobTitle}");
            var jobTitles = await _jobTitleService.JobTitlesAsync();
            
            var jobTitlesFiltered = jobTitles.Where(x => x.Contains(jobTitle)).ToList();
            return Ok(jobTitlesFiltered);
        }
    }

    class JobTitleAutocompleteResponse
    {
        [JsonProperty("job-titles")]
        public string[] JobTitles { get; set; }
    }
}