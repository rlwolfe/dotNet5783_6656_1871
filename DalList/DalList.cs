using BlApi;

namespace Dal
{
	sealed public class DalList : IDal
	{
		public IOrder Order => new DalOrder();

		public IProduct Product => new DalProduct();

		public IOrderItem OrderItem => new DalOrderItem();
	}
}
