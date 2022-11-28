using System.Diagnostics;
using System.Xml.Linq;

namespace DO;

/// <summary>
/// Structure for Product
/// </summary>
public struct Product
{
	/// <summary>
	/// Information about the Product
	/// </summary>
	/// product(){ ID=Config::prodcount++;?
	public int ID { get; set; }
	public int Name { get; set; }
	public int Category { get; set; }
	public int Price { get; set; }
	public int InStock { get; set; }

	public override string ToString() => $@"
			Product ID={ID}: {Name},
			category - {Category}
			Price: {Price}
			Amount in stock: {InStock}
	";

}
