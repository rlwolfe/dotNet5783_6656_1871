namespace BO
{
	public class ProductForList
	{
		public int m_id { get; set; }
		public string? m_name { get; set; }
		public BO.Enums.Category m_category { get; set; }
		public double m_price { get; set; }

		public override string ToString() => $"ID = {m_id}: Name - {m_name},\nCategory - {m_category},\nPrice: {m_price}";
	}
}
