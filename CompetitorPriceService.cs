using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class CompetitorPrice
{
    public string RoomType { get; set; }
    public string View { get; set; }
    public decimal Price { get; set; }
    public decimal BasePrice { get; set; }
    // Removed DateTime property since Date parsing is ignored
}

public class CompetitorPriceService
{
    private readonly string _csvPath;

    public CompetitorPriceService(string csvPath)
    {
        _csvPath = csvPath;
    }

    public async Task<decimal?> GetCompetitorPrice(RoomType type, ViewType view)
    {
        var viewString1 = view.ToString();
        var typeString1 = type.ToString();

        if (!File.Exists(_csvPath))
            return null;

        var lines = await File.ReadAllLinesAsync(_csvPath);
        var prices = new List<CompetitorPrice>();

        foreach (var line in lines.Skip(1)) // Skip header row
        {
            var parts = line.Split(',');

            if (parts.Length < 10) // Ensure we have enough columns
            {
                Console.WriteLine("Skipped line: not enough columns");
                continue;
            }

            // Debug: print the full parts array to ensure column alignment
            Console.WriteLine("DEBUG: Full line parts:");
            for (int i = 0; i < parts.Length; i++)
            {
                Console.WriteLine($"Column {i}: {parts[i].Trim()}");
            }

            var basePriceString = parts[5].Trim(); // Column F: BasePrice
            var competitorPriceString = parts[6].Trim(); // Column G: Competitor price
            var roomTypeString = parts[3].Trim(); // Column D: RoomType

            // Removed AvailableViews check as per your request

            // Declare viewString1 and typeString1 inside the loop
            // Debugging output for the current row's values
            Console.WriteLine($"DEBUG: RoomType={roomTypeString}, BasePrice={basePriceString}, CompetitorPrice={competitorPriceString}");

            // Match the room type (no need to check AvailableViews)
            if (!roomTypeString.Equals(typeString1, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Skipped: RoomType '{typeString1}' does not match '{roomTypeString}'");
                continue;
            }

            // Handle possible erroneous data in CompetitorPrice/Type (should be numeric)
            if (!decimal.TryParse(competitorPriceString, out var competitorPrice))
            {
                Console.WriteLine($"Failed to parse Competitor Price: {competitorPriceString}");
                continue;
            }

            // Parsing base price
            if (!decimal.TryParse(basePriceString, out var basePrice))
            {
                Console.WriteLine($"Failed to parse Base Price: {basePriceString}");
                continue;
            }

            // Add the valid competitor price entry to the list
            prices.Add(new CompetitorPrice
            {
                RoomType = typeString1,
                View = viewString1, // You can still store the view if needed
                Price = competitorPrice,
                BasePrice = basePrice,
            });

            Console.WriteLine($"Added: {typeString1} - {viewString1} - {competitorPrice}");
        }

        if (prices.Count == 0)
            return null;

        // Return the most recent competitor price (though we no longer care about date)
        return prices
            .OrderByDescending(p => p.BasePrice) // Just using BasePrice as a simple sorting criterion
            .First()
            .Price;
    }
}
