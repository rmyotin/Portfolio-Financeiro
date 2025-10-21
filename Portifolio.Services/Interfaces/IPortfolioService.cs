using Portifolio.Models.Models;

namespace Portifolio.Services.Interfaces
{
    public interface IPortfolioService
    {
        IEnumerable<Portfolio> GetAll();
        Portfolio? GetById(int id);
        (bool success, string message) Create(Portfolio portfolio);
        (bool success, string message) Update(int id, Portfolio updated);
        (bool success, string message) Delete(int id);
        (bool success, string message) AddPosition(int portfolioId, Position position);
        (bool success, string message) UpdatePosition(int positionId, Position updated);
        (bool success, string message) RemovePosition(int positionId);
        double CalculateTotalValue(Portfolio portfolio);
        double CalculateReturnPercent(Portfolio portfolio);
    }
}
