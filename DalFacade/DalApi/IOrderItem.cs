using DO;

namespace DalApi
{
	public interface IOrderItem :ICrud<OrderItem>
	{
		public OrderItem GetOrderItemWithProdAndOrderID(int productId, int orderId);
		public IEnumerable<OrderItem> GetItemsInOrder(int orderID);
		public void SetOrderItemWithProdAndOrderID(int productId, int orderId);
		public void SetItemsInOrder();
	}
}
