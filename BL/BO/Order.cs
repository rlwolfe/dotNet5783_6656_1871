using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	public class Order
	{
		public int m_id { get; set; }
		public string? m_customerName { get; set; }
		public string? m_customerEmail { get; set; }
		public string? m_customerAddress { get; set; }
		public DateTime? m_orderDate { get; set; }
		public Enums.OrderStatus m_status { get; set; }
		public DateTime? m_paymentDate { get; set; }
		public DateTime? m_shipDate { get; set; }
		public DateTime? m_deliveryDate { get; set; }
		public List<OrderItem?> m_items { get; set; }
		public double m_totalPrice { get; set; }

		public override string ToString()
		{
			string str = "";
			foreach (OrderItem item in m_items)         //creates string to add to ToString of list of items in order
			{
				str += item.ToString();
				str += "\n";
			};
			if (str == "")
				str = "none";

			return $@"Order ID = {m_id}:
			Customer Name - {m_customerName},
			Customer Email - {m_customerEmail},
			Customer Address - {m_customerAddress},
			Status - {m_status},
			Order Date: {m_orderDate},
			Payment Date: {m_paymentDate},
			Ship Date: {m_shipDate},
			Delivery Date: {m_deliveryDate},
			Total Price: {m_totalPrice}
			
			Items: {str}";
		}
	}
}
