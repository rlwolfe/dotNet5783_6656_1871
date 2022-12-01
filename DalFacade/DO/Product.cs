﻿using System.Diagnostics;
using System.Xml.Linq;

namespace DO;

/// <summary>
/// Structure for Product
/// </summary>
public struct Product
{
	public Product(int ID, string name, string category, double price, int inStock)
	{
		m_id = ID;
		m_name = name;
		m_category = category;
		m_price = price;
		m_inStock = inStock;
	}

	public int m_id { get; set; }
	public string m_name { get; set; }
	public string m_category { get; set; }
	public double m_price { get; set; }
	public int m_inStock { get; set; }

	public override string ToString() => $@"
			Product ID={m_id}: {m_name},
			category - {m_category}
			Price: {m_price}
			Amount in stock: {m_inStock}
	";

}