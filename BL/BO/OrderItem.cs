using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	public class OrderItem
	{
		public int m_id { get; set; }
		public int m_productID { get; set; }
		public int m_orderID { get; set; }
		public double m_price { get; set; }
		public int m_amount { get; set; }

		public override string ToString() => $@"
			Order Item ID = {m_id}
			Product ID = {m_productID},
			Order ID = {m_orderID}
			Price: {m_price}
			Amount: {m_amount}";
	}
}
