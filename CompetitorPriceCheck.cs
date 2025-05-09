using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class CompetitorPriceCheck
{
    private readonly HttpClient _httpClient;

    public CompetitorPriceCheck(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal?> GetCompetitorPrice(RoomType type, ViewType view)
    {
        var typeString = type.ToString(); // e.g., "Standard"
        var viewString = view.ToString(); // e.g., "Sea"

        var url = $"https://lord007tn.notion.site/901821b2c71e453097ca9f85919595fc?v=2347e90207364ae89b046000e819fd18&pvs=25//api/competitors/prices?type={typeString}&view={viewString}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        // assuming JSON response like: { "price": 120.0 }
        var data = await response.Content.ReadFromJsonAsync<CompetitorRoomResponse>();
        return data?.Price;
    }
}

public class CompetitorRoomResponse
{
    public decimal Price { get; set; }
}

