using static BO.Enums;

namespace BO
{
	public class OrderForList
	{
		public int m_id { get; set; }
		public string? m_customerName { get; set; }
		public OrderStatus m_status { get; set; }
		public double m_totalPrice { get; set; }
		public int m_amountOfItems { get; set; }

		public override string ToString() => $"ID = {m_id}:\nCustomer Name: {m_customerName},\nStatus - {m_status},\nTotal Price: {m_totalPrice}, Amount of Items: {m_amountOfItems}";
	}
}
