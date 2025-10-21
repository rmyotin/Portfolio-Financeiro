using Microsoft.EntityFrameworkCore;
using Portifolio.Infrastructure.Context;
using Portifolio.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Repositories.Repositories
{
    public class PortfolioRepository
    {
        private readonly AppDbContext _context;

        public PortfolioRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Portfolio> GetAll() =>
            _context.Portfolios.Include(p => p.Positions).ToList();

        public Portfolio? GetById(int id) =>
            _context.Portfolios.Include(p => p.Positions)
                               .FirstOrDefault(p => p.Id == id);

        public void Add(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();
        }

        public void Update(Portfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
            _context.SaveChanges();
        }

        public void Delete(Portfolio portfolio)
        {
            _context.Portfolios.Remove(portfolio);
            _context.SaveChanges();
        }

        public void AddPosition(int portfolioId, Position position)
        {
            var portfolio = GetById(portfolioId);
            if (portfolio == null) return;

            portfolio.Positions.Add(position);
            _context.SaveChanges();
        }

        public void UpdatePosition(Position position)
        {
            _context.Positions.Update(position);
            _context.SaveChanges();
        }

        public void RemovePosition(Position position)
        {
            _context.Positions.Remove(position);
            _context.SaveChanges();
        }

        public Position? GetPosition(int positionId)
        {
            return _context.Positions.FirstOrDefault(p => p.Id == positionId);
        }
    }
}
