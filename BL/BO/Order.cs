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
		public DateTime m_orderDate { get; set; }
		public DateTime m_shipDate { get; set; }
		public DateTime m_deliveryDate { get; set; }

		public override string ToString() => $@"
			Order ID = {m_id}
			Customer Name - {m_customerName},
			Customer Email - {m_customerEmail}
			Customer Address: {m_customerAddress}
			Order Date: {m_orderDate}
			Ship Date: {m_shipDate}
			Delivery Date: {m_deliveryDate}";
	}
}
