using System.Diagnostics;
using System.Xml.Linq;

namespace DO;

/// <summary>
/// Structure for OrderItem
/// </summary>
public struct OrderItem
{
	public OrderItem (int ID, int productID, int orderID, double Price, int amount)
	{
		m_id = ID;
		m_productID = productID;
		m_orderID = orderID;
		m_price = Price;
		m_amount = amount;
	}

	public int m_id { get; set; }
	public int m_productID { get; set; }
	public int m_orderID { get; set; }
	public double m_price { get; set; }
	public int m_amount { get; set; }

	public override string ToString() => $@"
			Product ID={m_id}: {m_productID},
			Order ID - {m_orderID}
			Price: {m_price}
			Amount: {m_amount}
	";
}
