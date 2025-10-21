using Newtonsoft.Json;
using Portifolio.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Tests
{
    public static class SeedDataLoader
    {
        public static SeedData Load()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "SeedData.json");
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SeedData>(json)!;
        }
    }
}
