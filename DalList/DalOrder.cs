using BlApi;
using DO;

namespace Dal;

///<summary>
///class for managing CRUD for Order
///</summary>
internal class DalOrder : IOrder
{
    ///<summary>
    /// Create function
    /// </summary>
    public int Create(Order order)
	{
		if (DataSource.Orders.Exists(x => x.m_id == order.m_id))
			throw new idAlreadyExistsException("Order");

		else if (DataSource.Orders.Count == DataSource.Orders.Capacity)
			Console.WriteLine("The orders list is already full");

		else
			DataSource.Orders.Add(order);

		return order.m_id;
	}

    ///<summary>
    /// Read function
    /// </summary>
    public Order Read(int ID)
	{
		if (DataSource.Orders.Exists(x => x.m_id == ID))
			return DataSource.Orders.Find(x => x.m_id == ID);

		throw new idNotFoundException("Order");
	}

	public IEnumerable<Order> ReadAll()
	{
		//maybe need to add new array and return that instead???
		return DataSource.Orders;
	}

    ///<summary>
    /// Update function
    /// </summary>
    public void Update(Order order)
	{
		if (DataSource.Orders.Exists(x => x.m_id == order.m_id))
			DataSource.Orders.Insert(DataSource.Orders.FindIndex(x => x.m_id == order.m_id), order);
		else
			throw new idNotFoundException("Order");
	}

    ///<summary>
    /// Delete function
    /// </summary>
    public void Delete(int ID)
	{
		if (DataSource.Orders.Exists(x => x.m_id == ID))
			DataSource.Orders.RemoveAt(DataSource.Orders.FindIndex(x => x.m_id == ID));

		else
			throw new idNotFoundException("Order");
	}
}
