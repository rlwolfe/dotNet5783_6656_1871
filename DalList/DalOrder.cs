using DO;
using System.Linq;

namespace Dal;

///<summary>
///class for managing CRUD for Order
///</summary>
public class DalOrder
{
    ///<summary>
    /// Create function
    /// </summary>
    public int CreateOrder(Order order)
	{

		if (order.m_id == -1)                                 //ID is null
			order.m_id = DataSource.Config.NextOrderNumber; //add 

		else if (Array.Exists(DataSource.Orders, x => x.m_id == order.m_id))
			throw new Exception("This order already exists");

		int i = 0;
		while (i < DataSource.Orders.Length)
		{
			if (DataSource.Orders[i].m_id == 0)
			{
				DataSource.Orders[i] = order;
				break;
			}
			else if (i == DataSource.Orders.Length - 1)
				Console.WriteLine("The array of orders is already full");
			i++;
		}
		return order.m_id;
	}

    ///<summary>
    /// Read function
    /// </summary>
    public Order ReadOrder(int ID)
	{
		if (Array.Exists(DataSource.Orders, x => x.m_id == ID))
			return DataSource.Orders[Array.IndexOf(DataSource.Orders, ID)];

		throw new Exception("An order with that ID was not found!");
	}

	public Order[] ReadAllOrders()
	{
		//maybe need to add new array and return that instead???
		return DataSource.Orders;
	}

    ///<summary>
    /// Update function
    /// </summary>
    public void UpdateOrder(Order order)
	{
		if (Array.Exists(DataSource.Orders, x => x.m_id == order.m_id))
			DataSource.Orders[Array.IndexOf(DataSource.Orders, order)] = order;
		else
			throw new Exception("Order ID doesn't exist");
	}

    ///<summary>
    /// Delete function
    /// </summary>
    public void DeleteOrder(int ID)
	{
		if (Array.Exists(DataSource.Orders, x => x.m_id == ID))
			DataSource.Orders = DataSource.Orders.Where(x => x.m_id != ID).ToArray<Order>();

		else
			throw new Exception("A order with that ID was not found!");
	}
}
