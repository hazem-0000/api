using System;

public enum RoomType
{
	Standard,
	Delux,
	Suite
}
public enum ViewType
{
	Sea,
	Garden,
	City
}

public class Room

{
	//public int Id {  get; set; }
	public RoomType Type { get; set; }
	public ViewType View { get; set; }
	public string Season { get; set; }
	public int OccupancyRate {  get; set; }
	
	
		
	public decimal BasePrice
	{
		get
		{
			return Type switch
			{
				RoomType.Standard => 500,
				RoomType.Delux => 150,
				RoomType.Suite => 250

			};
	}
	
	}

	}
	

	

