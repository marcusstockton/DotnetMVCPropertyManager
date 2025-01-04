using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
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
        private const bool DefaultForCacheRefreshNow = false;
        private const decimal DefaultCacheRefreshTimeOn24HourClockAsDecimalHours = 6.5m;

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
        private IMemoryCache _inMemoryCache;
        private static List<string> _cacheKeys = new List<string>();
        private static readonly object CacheLock = new object();

        public JobTitleServiceCache(IMemoryCache inMemoryCache)
        {
            _inMemoryCache = inMemoryCache;
        }

        public async Task<IList<string>> JobTitlesAsync()
        {
            var cachedValue = await _inMemoryCache.GetOrCreateAsync(
                "JobTitles",
                cacheEntry =>
                {
                    cacheEntry.SetSize(1000);
                    cacheEntry.SlidingExpiration = TimeSpan.FromHours(12);
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                    return GetJobTitles();
                });

            return cachedValue;

            //if (_inMemoryCache.TryGetValue("JobTitles", out IList<string> jobTitles))
            //{
            //    return jobTitles;
            //}

            //jobTitles = await GetJobTitles();
            //_inMemoryCache.Set("JobTitles", jobTitles);

            //_cacheKeys.Add("JobTitles");

            //return jobTitles;
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

        private class JobTitleAutocompleteResponse
        {
            [JsonProperty("job-titles")]
            public string[] JobTitles { get; set; }
        }
    }
}