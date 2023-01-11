using BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
	public interface IOrder
	{
		public int Create(Order order);
		public Order Read(int id);
		public IEnumerable<OrderForList?> ReadAll();
		public BO.Order UpdateOrderStatus(int orderId, BO.Enums.OrderStatus newStatus);
		public OrderTracking OrderTracker(int orderId);
	}
}