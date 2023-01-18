namespace BO
{
	public class ProductItem
	{
		public int m_id { get; set; }
		public string? m_name { get; set; }
		public BO.Enums.Category m_category { get; set; }
		public double m_price { get; set; }
		public bool m_inStock { get; set; }
		public int m_amount { get; set; }

		public override string ToString() => $"ID = {m_id}: Name - {m_name},\nCategory - {m_category}, Price: {m_price},\n{(m_inStock? $"{m_amount} In Stock" : "Not In Stock")}";
	}
}
