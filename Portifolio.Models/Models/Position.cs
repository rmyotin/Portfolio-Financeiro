using System.ComponentModel.DataAnnotations;

namespace Portifolio.Models.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AssetSymbol { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que 0.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Preço médio deve ser maior que 0.")]
        public double AveragePrice { get; set; }

        [Range(0, 1, ErrorMessage = "Alocação alvo deve estar entre 0 e 1.")]
        public double TargetAllocation { get; set; }

        public DateTime LastTransaction { get; set; } = DateTime.UtcNow;

        // FK opcional
        public int PortfolioId { get; set; }
    }
}
