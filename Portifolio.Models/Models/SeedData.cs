using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Models.Models
{
    public class SeedData
    {
        public List<Asset> Assets { get; set; } = new();
        public List<Portfolio> Portfolios { get; set; } = new();
    }
}
