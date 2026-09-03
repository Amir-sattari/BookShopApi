using System.Reflection;
using System.Text.Json;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Data
{
    public static class LocationSeeder
    {
        private const string ResourceName = "BookShopApi.Data.SeedData.iran-locations.json";

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var seed = LoadSeedData();
            if (seed.Count == 0)
                return;

            var existingProvinces = await db.Provinces
                .IgnoreQueryFilters()
                .Include(province => province.Cities)
                .ToListAsync();

            var provincesByName = existingProvinces
                .GroupBy(province => Normalize(province.Name))
                .ToDictionary(group => group.Key, group => group.First());

            foreach (var item in seed)
            {
                var provinceName = item.Name.Trim();
                if (string.IsNullOrEmpty(provinceName))
                    continue;

                if (!provincesByName.TryGetValue(Normalize(provinceName), out var province))
                {
                    province = new Province { Name = provinceName };
                    db.Provinces.Add(province);
                    provincesByName[Normalize(provinceName)] = province;
                }

                if (province.IsDeleted)
                    continue;

                var existingCityNames = province.Cities
                    .Select(city => Normalize(city.Name))
                    .ToHashSet();

                foreach (var cityName in item.Cities)
                {
                    var trimmedCityName = cityName.Trim();
                    if (string.IsNullOrEmpty(trimmedCityName))
                        continue;

                    var normalizedCityName = Normalize(trimmedCityName);
                    if (!existingCityNames.Add(normalizedCityName))
                        continue;

                    province.Cities.Add(new City { Name = trimmedCityName });
                }
            }

            await db.SaveChangesAsync();
        }

        private static List<ProvinceSeed> LoadSeedData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(ResourceName)
                ?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' was not found.");

            var seed = JsonSerializer.Deserialize<List<ProvinceSeed>>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return seed ?? [];
        }

        private static string Normalize(string value)
        {
            return value
                .Trim()
                .Replace('\u064a', '\u06cc')
                .Replace('\u0643', '\u06a9')
                .Replace("\u200c", string.Empty);
        }

        private sealed class ProvinceSeed
        {
            public string Name { get; set; } = string.Empty;
            public List<string> Cities { get; set; } = [];
        }
    }
}
