using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[Controller]")]
public class RoomController:ControllerBase
{
	private readonly RoomPricingService _roomPricingService;
    private readonly CompetitorPriceService _priceService;

   
    public RoomController(RoomPricingService pricingService)
	{
		_roomPricingService = pricingService;
        _priceService = new CompetitorPriceService("historical_booking_data.csv");
    }
    [HttpPost("rentroom")]
    public async Task<IActionResult> RentRoom(Room room, int numberOfDays)
    {
        // Check if the number of days is valid
        if (numberOfDays <= 0)
            return BadRequest("Number of days must be greater than 0");

        // Calculate our price using the RoomPricingService
        decimal price = _roomPricingService.CalculateTotalPrice(room, numberOfDays);

        // Fetch the competitor's price using the CompetitorPriceService
        var competitorPrice = await _priceService.GetCompetitorPrice(room.Type, room.View);

        // If no competitor price is found, return the calculated price with a message
        if (competitorPrice == null)
        {
            return Ok(new
            {
                RoomType = room.Type.ToString(),
                ViewType = room.View.ToString(),
                Season = room.Season.ToString(),
                NumberOfDays = numberOfDays,
                FinalPrice = price,
                Message = "No competitor price available"
            });
        }

        // Compare our price to the competitor's price using ComparePriceToCompetitor
        string priceComparison = _roomPricingService.ComparePriceToCompetitor(room, numberOfDays, competitorPrice.Value);

        // Return a response with all relevant information
        return Ok(new
        {
            RoomType = room.Type.ToString(),
            ViewType = room.View.ToString(),
            Season = room.Season.ToString(),
            NumberOfDays = numberOfDays,
            FinalPrice = price,
            CompetitorPrice = competitorPrice,
            PriceComparison = priceComparison
        });
    }

    [HttpGet("competitor")]
    public async Task<IActionResult> GetCompetitorPrice([FromQuery] RoomType type, [FromQuery] ViewType view)
    {
        var price = await _priceService.GetCompetitorPrice(type, view);
        if (price == null)
            return NotFound("No competitor price found for the specified type and view.");

        return Ok(new { RoomType = type, View = view, Price = price });
    }

}
