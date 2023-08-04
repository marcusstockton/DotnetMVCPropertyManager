using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Website.Data;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Portfolios;

namespace Website.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PortfolioService> _logger;
        private readonly IMapper _mapper;

        public PortfolioService(ApplicationDbContext context, ILogger<PortfolioService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public IQueryable<Portfolio> GetPortfolios()
        {
            _logger.LogInformation("Retrieving all portfolio's");
            var portfolios = _context.Portfolios.AsQueryable();
            return portfolios;
        }

        public async Task<IList<PortfolioDetailsDto>> GetMyPortfolios(string userId)
        {
            _logger.LogInformation($"{nameof(GetMyPortfolios)} > Retrieving all portfolio's for user id {userId}");
            var sw = new Stopwatch();
            sw.Start();
            var portfolios = await _context.Portfolios
                .Include(x => x.Owner)
                .Where(x => x.OwnerId == userId)
                .ProjectTo<PortfolioDetailsDto>(_mapper.ConfigurationProvider)
                //.AsNoTracking()
                .ToListAsync();
            sw.Stop();
            _logger.LogInformation($"{nameof(GetMyPortfolios)} took {sw.ElapsedMilliseconds}ms to complete");
            return portfolios;
        }

        public async Task<Portfolio> GetPortfolioById(Guid id)
        {
            _logger.LogInformation($"Retrieving portfolio with id {id}");
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
            _logger.LogInformation($"Deleting portfolio with id {portfolio.Id}");
            var properties = _context.Properties.Where(x => x.Portfolio.Id == portfolio.Id);
            _logger.LogInformation($"Also deleting all attached properties. Found {properties.Count()} properties to delete");
            foreach (var item in properties)
            {
                _context.Properties.Remove(item);
            }

            _context.Portfolios.Remove(portfolio);
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Sucessfully deleted portfolio with id {portfolio.Id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing property. {ex}");
                return false;
                throw;
            }
        }

        public async Task<Portfolio> UpdatePortfolio(Portfolio portfolio)
        {
            _logger.LogInformation($"Updating portfolio with id {portfolio.Id}");
            _context.Update(portfolio);
            try
            {
                await _context.SaveChangesAsync();
                return portfolio;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating property. {ex}");
                throw;
            }
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio, string username)
        {
            _logger.LogInformation($"Creating a new portfolio called '{portfolio.Name}' for user {username}");
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