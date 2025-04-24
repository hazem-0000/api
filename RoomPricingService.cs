using System;

public class RoomPricingService
{ 
	public decimal CalculateTotalPrice(Room room ,int NumberOfDays,string Season)
	{
		Decimal baseprice=room.Price;
		Decimal SeasonMultiplier = Season switch
		{
            "on-season" => 0.2m,
			"off-season" => 0
		};
		return baseprice*SeasonMultiplier*NumberOfDays;
	}
	
}
 