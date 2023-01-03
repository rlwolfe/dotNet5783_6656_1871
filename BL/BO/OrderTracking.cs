using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Enums;

namespace BO
{
	public class OrderTracking
	{
		public int m_id { get; set; }
		public OrderStatus m_status { get; set; }
		public List<Tuple<DateTime, string>>? DatePairs { get; set; }
		public override string ToString() => $@"
			Product ID = {m_id},
			Status: {m_status},
			Dates tracked: {DatePairs}";
	}
}
