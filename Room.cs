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
	Forest,
	City
}
public class Room
	
{
	public int Id {  get; set; }
	public RoomType Type  { get; set; }
	public ViewType View { get; set; }
	public decimal Price
	{
		get
		{
			return GetBasePrice() + GetViewBonus(); 

		}
	}
	private Decimal GetBasePrice()
	{
		return Type switch
		{
			RoomType.Standard => 100,
			RoomType.Delux => 200,
			RoomType.Suite => 300
		};
	}
		private Decimal GetViewBonus()
		{
			return View switch
			{
				ViewType.Sea => 30,
				ViewType.Forest => 20,
				ViewType.City => 10,
				_=>0
			};
		}
	}

	

