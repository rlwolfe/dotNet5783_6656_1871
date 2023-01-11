namespace BO
{
	public class Product
	{
        public int m_id { get; set; }
		public string? m_name { get; set; }
		public Enums.Category m_category { get; set; }
		public double m_price { get; set; }
		public int m_inStock { get; set; }

        public override string ToString() => $@"
			Product ID={m_id}: {m_name},
			Category - {m_category},
			Price: {m_price},
			Amount in stock: {m_inStock}";
	}
}
