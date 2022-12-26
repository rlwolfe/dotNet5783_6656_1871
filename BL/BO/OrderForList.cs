using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	internal class OrderForList
	{
		public int m_id { get; set; }
		public string? m_customerName { get; set; }
		public string? m_status { get; set; } //OrderStatus
		public double m_totalPrice { get; set; }
		public int m_amountOfItems { get; set; }

		public override string ToString() => $@"
			ID = {m_id}
			Customer Name: {m_customerName},
			Status: {m_status}
			Total Price: {m_totalPrice}
			Amount of Item: {m_amountOfItems}";
	}
}
