using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[Controller]")]
public class RoomController:ControllerBase
{
	private readonly RoomPricingService _roomPricingService;
	public RoomController(RoomPricingService pricingService)
	{
		_roomPricingService = pricingService;
	}
	[HttpPost("rentroom")]
	public IActionResult RentRoom(Room room,int numberOfDays,string season)
	{
		if (numberOfDays <= 0)
			return BadRequest("number of days must be greater than 0");
		Decimal price= _roomPricingService.CalculateTotalPrice(room,numberOfDays,season);
		return Ok(new
		{
			RoomType = room.Type.ToString(),
			ViewType = room.View.ToString(),
			numberOfDays = numberOfDays,
			FinalPrice = price
		});
	}
}
