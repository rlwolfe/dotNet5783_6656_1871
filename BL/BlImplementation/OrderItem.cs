using BlApi;
using Dal;

namespace BlImplementation
{
	internal class OrderItem : IOrderItem
	{
		private IDal? dal =  new DalList();

		public int Create(BO.OrderItem orderItem)
		{
		}

		public void Delete(int orderItemID)
		{
		}

		public IEnumerable<BO.OrderItem> GetItemsInOrder(int orderID)
		{
		}

		public BO.OrderItem GetOrderItemWithProdAndOrderID(int productId, int orderId)
		{
		}

		public BO.OrderItem Read(int id)
		{
		}

		public IEnumerable<BO.OrderItem> ReadAll()
		{
		}

		public void SetItemsInOrder()
		{
		}

		public void SetOrderItemWithProdAndOrderID(int productId, int orderId)
		{
		}

		public void Update(BO.OrderItem orderItem)
		{
		}
	}
}
