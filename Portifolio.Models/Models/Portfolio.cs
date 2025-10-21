using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Portifolio.Models.Models
{
    public class Portfolio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public double TotalInvestment { get; set; }

        [JsonPropertyName("createdAt")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Position> Positions { get; set; } = new List<Position>();
    }
}
