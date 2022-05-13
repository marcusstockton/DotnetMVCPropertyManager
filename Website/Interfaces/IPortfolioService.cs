using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Interfaces
{
    public interface IPortfolioService
    {
        IQueryable<Portfolio> GetPortfolios();

        Task<List<Portfolio>> GetMyPortfolios(string userId);

        Task<Portfolio> GetPortfolioById(Guid id);

        Task<bool> DeletePortfolio(Portfolio portfolio);

        Task<Portfolio> UpdatePortfolio(Portfolio portfolio);

        Task<Portfolio> CreatePortfolio(Portfolio portfolio, string username);
    }
}