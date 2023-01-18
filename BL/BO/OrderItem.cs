namespace BO
{
	public class OrderItem
	{
		public int m_id { get; set; }
		public int m_productID { get; set; }
		public double m_price { get; set; }
		public int m_amount { get; set; }

		public override string ToString() => $"Product ID = {m_productID}: {DalApi.Factory.Get().Product.Read(m_productID).m_name},\nPrice: {m_price}, Amount: {m_amount}";
		//OrderItem ID = {m_id}
	}
}
