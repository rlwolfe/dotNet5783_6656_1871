using BO;

namespace BlApi
{
	public interface IOrderItem
	{
		public void Create(OrderItem orderItem);
		public OrderItem Read(int id);
		public IEnumerable<OrderItem> ReadAll();
		public void Update(OrderItem orderItem);
		public void Delete(OrderItem orderItem);
		public OrderItem GetOrderItemWithProdAndOrderID(int productId, int orderId);
		public IEnumerable<OrderItem> GetItemsInOrder(int orderID);
		public void SetOrderItemWithProdAndOrderID(int productId, int orderId);
		public void SetItemsInOrder();
	}
}
