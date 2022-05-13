﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Website.Services
{
    public interface IJobTitleService
    {
        Task<IList<string>> JobTitlesAsync();
    }

    public interface ICacheSettings
    {
        bool ForceCacheRefreshNow { get; set; }
        decimal CacheRefreshTimeOn24HourClockAsDecimalHours { get; set; }
    }

    public class CacheSettings : ICacheSettings
    {
        const bool DefaultForCacheRefreshNow = false;
        const decimal DefaultCacheRefreshTimeOn24HourClockAsDecimalHours = 6.5m;

        public CacheSettings()
        {
            ForceCacheRefreshNow = DefaultForCacheRefreshNow;
            CacheRefreshTimeOn24HourClockAsDecimalHours = DefaultCacheRefreshTimeOn24HourClockAsDecimalHours;
        }
        public bool ForceCacheRefreshNow { get; set; }

        public decimal CacheRefreshTimeOn24HourClockAsDecimalHours { get; set; }
    }

    public class JobTitleServiceCache : IJobTitleService
    {
        IMemoryCache _inMemoryCache;
        static List<string> _cacheKeys = new List<string>();
        static readonly object CacheLock = new object();

        public JobTitleServiceCache(IMemoryCache inMemoryCache)
        {
            _inMemoryCache = inMemoryCache;
        }

        public async Task<IList<string>> JobTitlesAsync()
        {
            if (_inMemoryCache.TryGetValue("JobTitles", out IList<string> jobTitles))
            {
                return jobTitles;
            }

            jobTitles = await GetJobTitles();

            _inMemoryCache.Set("JobTitles", jobTitles);

            _cacheKeys.Add("JobTitles");

            return jobTitles;

        }

        private async Task<IList<string>> GetJobTitles()
        {
            var url = "https://raw.githubusercontent.com/jneidel/job-titles/master/job-titles.json";
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<JobTitleAutocompleteResponse>(data);
                return result.JobTitles.ToList();
            }
        }
        class JobTitleAutocompleteResponse
        {
            [JsonProperty("job-titles")]
            public string[] JobTitles { get; set; }
        }
    }
}
