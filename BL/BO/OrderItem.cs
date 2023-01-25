using System.Reflection;

namespace BO
{
	public class OrderItem
	{
		public OrderItem() { }

		public OrderItem(DO.OrderItem doOrderItem) {
			m_id= doOrderItem.m_id;
			m_productID = doOrderItem.m_productID;
			m_price = doOrderItem.m_price;
			m_amount = doOrderItem.m_amount;
		}
		public int m_id { get; set; }
		public int m_productID { get; set; }
		public double m_price { get; set; }
		public int m_amount { get; set; }

		public override string ToString() => $"Product ID = {m_productID}: {DalApi.Factory.Get().Product.Read(m_productID).m_name},\nPrice: {m_price}, Amount: {m_amount}";
		//OrderItem ID = {m_id}
	}
}
