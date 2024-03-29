﻿namespace DO;

/// <summary>
/// Struct for Order
/// </summary>
public struct Order
{
	static int idCounter = 1;
	public Order(string customerName, string customerEmail, string customerAddress, DateTime? orderDate,
		DateTime? shipDate, DateTime? deliveryDate)
	{
		m_id = idCounter++;
		m_customerName = customerName;
		m_customerEmail = customerEmail;
		m_customerAddress = customerAddress;
		m_orderDate = orderDate;
		m_shipDate = shipDate;
		m_deliveryDate = deliveryDate;
	}

	public int m_id { get; set; }
	public string? m_customerName { get; set; }
	public string? m_customerEmail { get; set; }
	public string? m_customerAddress { get; set; }
	public DateTime? m_orderDate { get; set; }
	public DateTime? m_shipDate { get; set; }
	public DateTime? m_deliveryDate { get; set; }

	public override string ToString() => $@"
			Order ID = {m_id}:
			Customer Name - {m_customerName},
			Customer Email - {m_customerEmail},
			Customer Address - {m_customerAddress},
			Order Date: {m_orderDate},
			Ship Date: {m_shipDate},
			Delivery Date: {m_deliveryDate}";
}
