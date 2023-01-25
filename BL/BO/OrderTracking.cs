using static BO.Enums;

namespace BO
{
	public class OrderTracking
	{
		public int m_id { get; set; }
		public OrderStatus m_status { get; set; }
		public List<Tuple<DateTime?, string?>>? DatePairs { get; set; }
		public override string ToString() => $"{DatePairs.Last().Item2} on {DatePairs.Last().Item1.Value.ToShortDateString()}";
	}
}
