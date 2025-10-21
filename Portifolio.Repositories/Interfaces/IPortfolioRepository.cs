using Portifolio.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Repositories.Interfaces
{
    public interface IPortfolioRepository
    {
        IEnumerable<Portfolio> GetAll();
        Portfolio? GetById(int id);
        void Add(Portfolio portfolio);
        void Update(Portfolio portfolio);
        void Delete(Portfolio portfolio);
        void AddPosition(int portfolioId, Position position);
        void UpdatePosition(Position position);
        Position? GetPosition(int positionId);
        void RemovePosition(Position position);

    }
}
