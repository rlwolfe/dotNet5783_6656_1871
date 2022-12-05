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
		
		int i = 0;
		while (i < DataSource.OrderItems.Length)
		{
			if (DataSource.OrderItems[i].m_id == 0)
			{
				DataSource.OrderItems[i] = orderItem;
				break;
			}
			else if (i == DataSource.OrderItems.Length - 1)
				Console.WriteLine("The array of order items is already full");
			i++;
		}
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

		OrderItem[] tempList = DataSource.OrderItems.TakeWhile(x => x.m_productID == productId && x.m_orderID == orderID).ToArray<OrderItem>();

		if (tempList != null)
			return tempList.First();

		/*for (int i = 0; i < DataSource.OrderItems.Length; i++)
		{
			if (DataSource.OrderItems[i].m_productID == productId && DataSource.OrderItems[i].m_orderID == orderID)
				return DataSource.OrderItems[i];
		}*/
		throw new Exception("This order item doesn't exist");
	}

	public void SetOrderItemWithProdAndOrderID(int productId, int orderId)
	{
		Product tempProd;
		Product[] productList = DataSource.Products.Where(x => x.m_id == productId).ToArray<Product>();
		
		if (productList != null)
			tempProd = productList.First();
		else
			throw new Exception("This product ID doesn't exist");

		OrderItem orderItem = new OrderItem(-1, productId, orderId, tempProd.m_price, 1);   //how to determine the amount?
		CreateOrderItem(orderItem);																									
	}

	public OrderItem[] GetItemsInOrder(int orderID)
	{
		OrderItem[] itemsInOrder = DataSource.OrderItems.TakeWhile(x => x.m_orderID == orderID).ToArray<OrderItem>();
		
		if (itemsInOrder == null)
			throw new Exception("No items in that order!");		//return this instead?

		return itemsInOrder;
	}

	public void SetItemsInOrder()
	{
		//he's going to tell us
	}

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
