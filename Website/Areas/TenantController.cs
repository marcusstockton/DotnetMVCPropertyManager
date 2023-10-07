using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.Models.DTOs.Tenants;
using Website.Services;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class TenantController : ControllerBase
    {
        private readonly ILogger<TenantController> _logger;
        private readonly IMapper _mapper;
        private readonly IJobTitleService _jobTitleService;
        private readonly ITenantService _tenantService;

        public TenantController(ILogger<TenantController> logger, IMapper mapper, IJobTitleService jobTitleService, ITenantService tenantService)
        {
            _logger = logger;
            _mapper = mapper;
            _jobTitleService = jobTitleService;
            _tenantService = tenantService;
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

        [Obsolete("Not currently in use")]
        [HttpGet("GetTenantsForProperty/{propertyId}")]
        public async Task<ActionResult<IEnumerable<TenantDTO>>> GetTenantsForProperty(Guid propertyId)
        {
            var tenants = await _tenantService.GetTenantsForPropertyId(propertyId);
            var result = _mapper.Map<IEnumerable<TenantDTO>>(tenants);
            return Ok(result);
        }
    }

    class JobTitleAutocompleteResponse
    {
        [JsonProperty("job-titles")]
        public string[] JobTitles { get; set; }
    }
}