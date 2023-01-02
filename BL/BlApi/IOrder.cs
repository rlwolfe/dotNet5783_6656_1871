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
		public IEnumerable<Order> ReadAll();
		public void Update(Order order);
		public void Delete(int orderId);
	}
}
