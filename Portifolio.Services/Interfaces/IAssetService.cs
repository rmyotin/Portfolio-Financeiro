using Portifolio.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Services.Interfaces
{
    public interface IAssetService
    {
        IEnumerable<Asset> GetAll();
        Asset? GetById(int id);
        Asset? GetBySymbol(string symbol);
        (bool success, string message) Create(Asset asset);
        (bool success, string message) Update(int id, Asset updatedAsset);
        (bool success, string message) UpdatePrice(int id, double newPrice);
        (bool success, string message) Delete(string symbol);

    }
}
