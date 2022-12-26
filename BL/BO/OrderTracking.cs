using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	internal class OrderTracking
	{
		public int m_id { get; set; }
		public string? m_status { get; set; } //OrderStatus
		public override string ToString() => $@"
			Product ID = {m_id},
			Status: {m_status}";
	}
}
