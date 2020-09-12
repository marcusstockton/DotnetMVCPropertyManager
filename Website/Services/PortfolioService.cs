using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Data;
using Website.Interfaces;
using Website.Models;

namespace Website.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;

        public PortfolioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Portfolio>> GetPortfolios()
        {
            return await _context.Portfolios.ToListAsync();
        }

        public async Task<List<Portfolio>> GetMyPortfolios(string userId)
        {
            return await _context.Portfolios
                .Include(x => x.Owner)
                .Where(x => x.Owner.Id == userId)
                .ToListAsync();
        }

        public async Task<Portfolio> GetPortfolioById(Guid id)
        {
            var portfolio = await _context.Portfolios
                .Include(owner => owner.Owner)
                .Include(portfolio => portfolio.Properties)
                    .ThenInclude(property => property.Address)
                .Include(x => x.Properties)
                    .ThenInclude(y => y.Tenants)
                .SingleAsync(x => x.Id == id);

            return portfolio;
        }

        public async Task<bool> DeletePortfolio(Portfolio portfolio)
        {
            var properties = _context.Properties.Where(x => x.Portfolio.Id == portfolio.Id);
            foreach (var item in properties)
            {
                _context.Properties.Remove(item);
            }

            _context.Portfolios.Remove(portfolio);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<Portfolio> UpdatePortfolio(Portfolio portfolio)
        {
            _context.Update(portfolio);
            try
            {
                await _context.SaveChangesAsync();
                return portfolio;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio, string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                return null;
            }
            portfolio.Owner = user;
            _context.Add(portfolio);
            try
            {
                await _context.SaveChangesAsync();
                return portfolio;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}