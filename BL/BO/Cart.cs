using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	public class Cart
	{
		public string? m_customerName { get; set; }
		public string? m_customerEmail { get; set; }
		public string? m_customerAddress { get; set; }

		public List<OrderItem> m_items = new List<OrderItem>();
		public double m_totalPrice { get; set; }

		public override string ToString()
		{
			string str = "";
			foreach (OrderItem item in m_items)
			{
				str += item.ToString();
				str += "\n";
			};
			if (str == "")
				str = "none";

			return $@"Customer's Name - {m_customerName},
			Customer's Email - {m_customerEmail},
			Customer's Address - {m_customerAddress},
			Total Price: {m_totalPrice},

			List of Items in cart: {str}";
		}
	}
}