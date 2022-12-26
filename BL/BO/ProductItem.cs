using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	internal class ProductItem
	{
		public int m_id { get; set; }
		public string? m_name { get; set; }
		public string? m_category { get; set; } //Category
		public double m_price { get; set; }
		public bool m_inStock { get; set; }
		public int m_amount { get; set; }

		public override string ToString() => $@"
			ID = {m_id}
			Name: {m_name},
			Category - {m_category},
			Price: {m_price},
			Amount: {m_amount},
			In Stock? {m_inStock}";
	}
}
