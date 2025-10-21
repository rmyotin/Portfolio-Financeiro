using Portifolio.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Repositories.Interfaces
{
    public interface IAssetRepository
    {
        IEnumerable<Asset> GetAll();
        void Add(Asset asset);
        Asset? GetById(int id);
        Asset? GetBySymbol(string symbol);
        void Update(Asset asset);
        void Delete(string symbol);
        bool Exists(int id);
        bool ExistsBySymbol(string symbol);

    }
}
