using static BO.Enums;

namespace BO
{
	public class OrderTracking
	{
		public int m_id { get; set; }
		public OrderStatus m_status { get; set; }
		public List<Tuple<DateTime?, string?>>? DatePairs { get; set; }
		public override string ToString()
		{
			string str = "";
			foreach (Tuple<DateTime?, string?> pair in DatePairs)			//creates string to add to ToString of list of tuples
			{
				str += pair.ToString();
				str += "\n";
			};
			if (str == "")
				str = "No dates to track";

			return $@"Product ID = {m_id},
			Status - {m_status},
			Dates tracked: {str}";
		}
	}
}
