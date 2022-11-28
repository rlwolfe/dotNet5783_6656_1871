namespace DO;

/// <summary>
/// Structure for Order
/// </summary>
public struct Order
{
	/// <summary>
	/// Information about the Order
	/// </summary>
	public int ID { get; set; }
	public int CustomerName { get; set; }
	public int CustomerEmail { get; set; }
	public int CustomerAddress { get; set; }
	public int OrderDate { get; set; }
	public int ShipDate { get; set; }
	public int DeliveryDate { get; set; }

	public override string ToString() => $@"
			Product ID={ID}: {CustomerName},
			Customer Email - {CustomerEmail}
			Customer Address: {CustomerAddress}
			Order Date: {OrderDate}
			Ship Date: {ShipDate}
			Delivery Date: {DeliveryDate}
	";
}
