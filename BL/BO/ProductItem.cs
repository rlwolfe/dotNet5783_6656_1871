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

		public override string ToString() => $@"
			ID = {m_id}:
			Name: {m_name},
			Category - {m_category},
			Price: {m_price},
			Amount: {m_amount},
			In Stock? {m_inStock}";
	}
}
