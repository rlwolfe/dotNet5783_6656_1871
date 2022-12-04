using DO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dal;

public class DalOrderItem
{
	public int CreateOrderItem(OrderItem orderItem)
	{

		if (orderItem.m_id == -1)                                 //ID is null
			orderItem.m_id = DataSource.Config.NextOrderItemNumber; //add 

		else if (Array.Exists(DataSource.OrderItems, x => x.m_id == orderItem.m_id))
			throw new Exception("This order item already exists");

		DataSource.OrderItems.Append(orderItem);

		return orderItem.m_id;

	}

	public OrderItem ReadOrderItem(int ID)
	{
		if (Array.Exists(DataSource.OrderItems, x => x.m_id == ID))
			return DataSource.OrderItems[Array.IndexOf(DataSource.OrderItems, ID)];

		throw new Exception("An order item with that ID was not found!");
	}

	public OrderItem[] ReadAllOrderItems()
	{
		//maybe need to add new array and return that instead???
		return DataSource.OrderItems;
	}

	public OrderItem GetOrderItemWithProdAndOrderID(int productId, int orderID)
	{
		OrderItem[] tempList = DataSource.OrderItems.TakeWhile(x => x.m_productID == productId).ToArray<OrderItem>();
		
		if (tempList != null)
			if (Array.Exists(tempList, x => x.m_orderID == orderID))
				return Array.Find(tempList, x => x.m_orderID == orderID);

		throw new Exception("This order item doesn't exist");
	}

	/*public void SetOrderItemWithProdAndOrderID(int productId, int orderID)
	{//we need other data to set
		OrderItem[] tempList = DataSource.OrderItems.TakeWhile(x => x.m_productID == productId).ToArray<OrderItem>();

		if (tempList != null)
			if (Array.Exists(tempList, x => x.m_orderID == orderID))

		throw new Exception("This order item doesn't exist");
	}*/

	public OrderItem[] GetItemsInOrder(int orderID)
	{
		OrderItem[] itemsInOrder = DataSource.OrderItems.TakeWhile(x => x.m_orderID == orderID).ToArray<OrderItem>();
		
		if (itemsInOrder == null)
			throw new Exception("No items in that order!");

		return itemsInOrder;
	}

	//set for this ^?

	public void UpdateOrderItem(OrderItem orderItem)
	{
		if (Array.Exists(DataSource.OrderItems, x => x.m_id == orderItem.m_id))

			DataSource.OrderItems[Array.IndexOf(DataSource.OrderItems, orderItem)] = orderItem;
		else
			throw new Exception("Order Item ID doesn't exist");
	}

	public void DeleteOrderItem(int ID)
	{
		if (Array.Exists(DataSource.OrderItems, x => x.m_id == ID))
			DataSource.OrderItems = DataSource.OrderItems.Where(x => x.m_id != ID).ToArray<OrderItem>();

		else
			throw new Exception("A order item with that ID was not found!");
	}
}
