using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Models.Models
{
    public class Portfolio
    {
        [Key]
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public double TotalInvestment { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Position> Positions { get; set; } = new();
    }
}
