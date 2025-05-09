using System;
public enum Occupancy
{
    Low,
    Medium,
    High
}

public class RoomPricingService
{
    public static Occupancy GetOccupancyLevel(int rate)
    {
        if (rate <= 30) return Occupancy.Low;
        else if (rate <= 70) return Occupancy.Medium;
        else return Occupancy.High;
    }
    public decimal CalculateTotalPrice(Room room ,int NumberOfDays)
	{
      
        Decimal baseprice=room.BasePrice;
		Decimal SeasonMultiplier = room.Season switch
		{
            "Peakseason" => 0.3m,
			"off-season" => -0.2m
		};


        var level = GetOccupancyLevel(room.OccupancyRate);
        Decimal OccupancyMultiplier = level switch
		{
			Occupancy.Low => -0.1m,
			Occupancy.Medium => 0,
			Occupancy.High => 0.2m
		};


		
		return baseprice+( baseprice*SeasonMultiplier*NumberOfDays)+(baseprice*OccupancyMultiplier);
	}
    public string ComparePriceToCompetitor(Room room, int numberOfDays, decimal competitorPrice)
    {
        decimal ourPrice = CalculateTotalPrice(room, numberOfDays);

        if (ourPrice < competitorPrice)
        {
            return "Our price is lower.";
        }
        else if (ourPrice > competitorPrice)
        {
            return "Our price is higher.";
        }
        else
        {
            return "Our price is the same as the competitor's price.";
        }
    }
}
 