using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Models.Models
{
    public class Position
    {
        public string AssetSymbol { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double AveragePrice { get; set; }
        public double TargetAllocation { get; set; }
        public DateTime LastTransaction { get; set; }
    }
}
