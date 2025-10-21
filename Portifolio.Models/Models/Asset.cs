using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Models.Models
{
    public class Asset
    {
        [Key]
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public double CurrentPrice { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
