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
			public List<OrderItem> m_items{ get; set; }
			public double m_totalPrice { get; set; }

			public override string ToString() => $@"
			Customer's Name: {m_customerName},
			Customer's Email: {m_customerEmail},
			Customer's Address: {m_customerAddress},
			List of Items in cart - {m_items},
			Total Price: {m_totalPrice}";
	}
}