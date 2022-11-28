namespace DO;

/// <summary>
/// Structure for OrderItem
/// </summary>
public struct OrderItem
{
	/// <summary>
	/// Information about the OrderItem
	/// </summary>
	public int ID { get; set; }
	public int ProductID { get; set; }
	public int OrderID { get; set; }
	public int Price { get; set; }
	public int Amount { get; set; }

	public override string ToString() => $@"
			Product ID={ID}: {ProductID},
			Order ID - {OrderID}
			Price: {Price}
			Amount: {Amount}
	";
}
