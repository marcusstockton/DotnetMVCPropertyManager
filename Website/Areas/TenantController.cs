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
        private readonly IJobTitleService _jobTitleService;
        private readonly ITenantService _tenantService;

        public TenantController(ILogger<TenantController> logger, IJobTitleService jobTitleService, ITenantService tenantService)
        {
            _logger = logger;
            _jobTitleService = jobTitleService;
            _tenantService = tenantService;
        }

        // /api/Tenant/job-title-autocomplete?jobTitle="software"
        [HttpGet("job-title-autocomplete")]
        public async Task<ActionResult> JobTitleAutoComplete(string jobTitle)
        {
            _logger.LogInformation($"{nameof(JobTitleAutoComplete)} finding job titles with {jobTitle}");
            var jobTitles = await _jobTitleService.JobTitlesAsync();

            var jobTitlesFiltered = jobTitles.Where(x => x.Contains(jobTitle, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(jobTitlesFiltered);
        }

        [Obsolete("Not currently in use")]
        [HttpGet("GetTenantsForProperty/{propertyId}")]
        public async Task<ActionResult<IEnumerable<TenantDTO>>> GetTenantsForProperty(Guid propertyId)
        {
            var tenants = await _tenantService.GetTenantsForPropertyId(propertyId);

            var results = tenants.Select(t => new List<TenantDTO>
            {
                new TenantDTO
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    DateOfBirth = t.DateOfBirth,
                    EmailAddress = t.EmailAddress,
                    PhoneNumber = t.PhoneNumber,
                    JobTitle = t.JobTitle,
                    NationalityId = t.Nationality.Id,
                    IsSmoker = t.IsSmoker,
                    HasPets = t.HasPets,
                    CreatedDate = t.CreatedDate,
                    Nationality = t.Nationality,
                    Notes = t.Notes,
                    Property = t.Property,
                    TenancyEndDate = t.TenancyEndDate,
                    TenancyStartDate = t.TenancyStartDate,
                    TenantImage = t.TenantImage,
                    UpdatedDate = t.UpdatedDate.GetValueOrDefault().DateTime
                } }).ToList();

            return Ok(results);
        }
    }

    internal class JobTitleAutocompleteResponse
    {
        [JsonProperty("job-titles")]
        public string[] JobTitles { get; set; }
    }
}