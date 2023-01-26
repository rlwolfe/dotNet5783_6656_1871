using DalApi;
using DO;
using System.Linq;

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
		if (DataSource.OrderItems.Exists(x => x?.m_id == orderItem.m_id))
			throw new idAlreadyExistsException { };

		else if (DataSource.OrderItems.Count == DataSource.OrderItems.Capacity)
			Console.WriteLine("The order items list is already full");

		else
			DataSource.OrderItems.Add(orderItem);

		return orderItem.m_id;
	}

	///<summary>
	/// Read function
	/// </summary>
	public OrderItem Read(int ID)
	{
		if (DataSource.OrderItems.Exists(x => x?.m_id == ID))
			return DataSource.OrderItems.Find(x => x?.m_id == ID).Value;

		throw new idNotFoundException { };
	}
	public OrderItem ReadWithFilter(Func<OrderItem?, bool>? filter)
	{
		if (filter == null)
		{
			throw new ArgumentNullException(nameof(filter));//filter is null
		}

		foreach (var orderItem in from OrderItem? orderItem in DataSource.OrderItems
								  where orderItem != null && filter(orderItem)
								  select orderItem)
			return (OrderItem)orderItem;

		throw new DO.EntityNotFoundException();
	}
	public IEnumerable<OrderItem?> ReadAllFiltered(Func<OrderItem?, bool>? filter)
	{
		if (filter == null)                                 //return all
			return DataSource.OrderItems;

		return from orderItem in DataSource.OrderItems      //return filtered list
			   where filter(orderItem) select orderItem;
	}

	///<summary>
	/// Update function
	/// </summary>
	public void Update(OrderItem orderItem)
	{
		if (DataSource.OrderItems.Exists(x => x?.m_id == orderItem.m_id))
			DataSource.OrderItems.Insert(DataSource.Orders.FindIndex(x => x?.m_id == orderItem.m_id), orderItem);
		else
			throw new idNotFoundException { };
	}

	///<summary>
	/// Read function for when given Product ID and OrderID
	/// </summary>
	public OrderItem GetOrderItemWithProdAndOrderID(int productId, int orderId)
	{

		List<OrderItem> tempList = (List<OrderItem>)DataSource.OrderItems.Where(x => x?.m_productID == productId && x?.m_orderID == orderId);

		if (tempList.Count != 0)
			return tempList.First();

		throw new EntityNotFoundException { };
	}

	///<summary>
	/// Update function for when given Product ID and OrderID
	/// </summary>
	public void SetOrderItemWithProdAndOrderID(int productId, int orderId) //OrderItem SetOrderItemWithProdAndOrderID(int productId, int orderId)
	{//how to determine what to change? He's getting back to us
		List<OrderItem> tempList = (List<OrderItem>)DataSource.OrderItems.Where(x => x?.m_productID == productId && x?.m_orderID == orderId);

		//if (tempList.Length == 0){
		//OrderItem orderItem = new OrderItem(productId, orderId, new Random(5).NextDouble(), new Random(8).Next());
		//Create(orderItem);
		//}
		if (tempList.Count != 0)
		{
			Update(tempList.First());
			//return tempList.First()
		}
		else
			throw new EntityNotFoundException { };
	}

	///<summary>
	/// Read function for all order items in single order
	/// </summary>
	public IEnumerable<OrderItem?> GetItemsInOrder(int orderID)
	{
		List<OrderItem?> itemsInOrder = (List<OrderItem?>)DataSource.OrderItems.Where(x => x?.m_orderID == orderID);

		if (itemsInOrder.Count == 0)
			throw new idNotFoundException { };

		return itemsInOrder;
	}

	///<summary>
	/// Insufficient data given for use case, return variable or parameters of function
	///- on hold while professor gets more info
	/// </summary>
	public void SetItemsInOrder(int orderID)
	{//he's going to tell us
		List<OrderItem> tempList = (List<OrderItem>)DataSource.OrderItems.Where(x => x?.m_orderID == orderID);

		/*if (tempList.Length == 0){
			OrderItem orderItem = new OrderItem(???, orderId, new Random(5).NextDouble(), new Random(8).Next());
			Create(orderItem);
		}*/
	}

	///<summary>
	/// Delete function
	/// </summary>
	public void Delete(int ID)
	{
		if (DataSource.OrderItems.Exists(x => x?.m_id == ID))
			DataSource.OrderItems.RemoveAt(DataSource.OrderItems.FindIndex(x => x?.m_id == ID));

		else
			throw new idNotFoundException { };
	}
}
