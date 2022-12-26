using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	internal class ProductForList
	{
		public int m_id { get; set; }
		public string? m_name { get; set; }
		public string? m_category { get; set; }
		public double m_price { get; set; }

		public override string ToString() => $@"
			ID = {m_id}
			Name: {m_name},
			Category - {m_category}
			Price: {m_price}";
	}
}
