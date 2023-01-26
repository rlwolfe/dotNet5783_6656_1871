using DalApi;


namespace Dal
{
    sealed public class DalList : IDal
	{
		public static IDal Instance { get; } = new DalList();

		private DalList() { }

		public IOrder Order => new DalOrder();

		public IProduct Product => new DalProduct();

		public IOrderItem OrderItem => new DalOrderItem();
	}
}
