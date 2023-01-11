namespace BO
{
	public class OrderItem
	{
		public int m_id { get; set; }
		public int m_productID { get; set; }
		public double m_price { get; set; }
		public int m_amount { get; set; }

		public override string ToString() => $@"
			Order Item ID = {m_id}:
			Product ID = {m_productID},
			Price: {m_price},
			Amount: {m_amount}";
	}
}
