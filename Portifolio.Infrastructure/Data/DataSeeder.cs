using Newtonsoft.Json;
using Portifolio.Infrastructure.Context;
using Portifolio.Models.Models;

namespace Portifolio.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Assets.Any()) return;

            var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException("SeedData.json não encontrado em " + jsonPath);

            var json = File.ReadAllText(jsonPath);
            var data = JsonConvert.DeserializeObject<SeedData>(json)!;

            context.Assets.AddRange(data.Assets);
            context.Portfolios.AddRange(data.Portfolios);
            context.SaveChanges();
        }
    }
}
