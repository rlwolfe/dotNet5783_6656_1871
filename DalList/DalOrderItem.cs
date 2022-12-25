using BlApi;
using DO;

namespace Dal;

/// <summary>
/// Class for managing CRUD for OrderItem
/// </summary>
internal class DalOrderItem : IOrderItem
{
    ///<summary>
    /// Create function
    /// </summary>
    public int Create(OrderItem orderItem)
	{
		if (DataSource.OrderItems.Exists(x => x.m_id == orderItem.m_id))
			throw new idAlreadyExistsException("Order Item");

		else if (DataSource.OrderItems.Count == DataSource.OrderItems.Capacity)
			Console.WriteLine("The order items list is already full");

		else
			DataSource.OrderItems.Add(orderItem);

		return orderItem.m_id;;
	}

    ///<summary>
    /// Read function
    /// </summary>
    public OrderItem Read(int ID)
	{
		if (DataSource.OrderItems.Exists(x => x.m_id == ID))
			return DataSource.OrderItems.Find(x => x.m_id == ID);

		throw new idNotFoundException("Order Item");
	}

	public IEnumerable<OrderItem> ReadAll()
	{
		return DataSource.OrderItems;
	}

    ///<summary>
    /// Update function
    /// </summary>
    public void Update(OrderItem orderItem)
	{
		if (DataSource.OrderItems.Exists(x => x.m_id == orderItem.m_id))
			DataSource.OrderItems.Insert(DataSource.Orders.FindIndex(x => x.m_id == orderItem.m_id), orderItem);
		else
			throw new idNotFoundException("Order Item");
	}

    ///<summary>
    /// Read function for when given Product ID and OrderID
    /// </summary>
    public OrderItem GetOrderItemWithProdAndOrderID(int productId, int orderId)
	{

		OrderItem[] tempList = DataSource.OrderItems.Where(x => x.m_productID == productId && x.m_orderID == orderId).ToArray<OrderItem>();

		if (tempList.Length != 0)
			return tempList.First();

		/*for (int i = 0; i < DataSource.OrderItems.Length; i++)
		{
			if (DataSource.OrderItems[i].m_productID == productId && DataSource.OrderItems[i].m_orderID == orderID)
				return DataSource.OrderItems[i];
		}*/
		throw new EntityNotFoundException("Order Item");
	}

    ///<summary>
    /// Update function for when given Product ID and OrderID
    /// </summary>
    public void SetOrderItemWithProdAndOrderID(int productId, int orderId)
	{//how to determine what to change
		OrderItem[] tempList = DataSource.OrderItems.Where(x => x.m_productID == productId && x.m_orderID == orderId).ToArray<OrderItem>();
		
		if (tempList.Length != 0)
			Update(tempList.First());
		else
			throw new EntityNotFoundException("Order Item");																							
	}

    ///<summary>
    /// Read function for all order items in single order
    /// </summary>
    public IEnumerable<OrderItem> GetItemsInOrder(int orderID)
	{
		OrderItem[] itemsInOrder = DataSource.OrderItems.Where(x => x.m_orderID == orderID).ToArray<OrderItem>();
		
		if (itemsInOrder.Length == 0)
			throw new idNotFoundException("Order");

		return itemsInOrder;
	}

    ///<summary>
    /// Insufficient data given for use case, return variable or parameters of function
	///- on hold while professor gets more info
    /// </summary>
    public void SetItemsInOrder()
	{
		//he's going to tell us
	}

    ///<summary>
    /// Delete function
    /// </summary>
    public void Delete(int ID)
	{
		if (DataSource.OrderItems.Exists(x => x.m_id == ID))
			DataSource.OrderItems.RemoveAt(DataSource.OrderItems.FindIndex(x => x.m_id == ID));

		else
			throw new idNotFoundException("Order Item");
	}
}
