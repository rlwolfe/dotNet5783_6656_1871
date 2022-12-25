using BlApi;
using Dal;

namespace BlImplementation
{
	internal class Order : IOrder
	{
		private IDal? dal = new DalList();

		public void Create(BO.Order order)
		{
		}

		public void Delete(BO.Order order)
		{
		}

		public BO.Order Read(int id)
		{
		}

		public IEnumerable<BO.Order> ReadAll()
		{
		}

		public void Update(BO.Order order)
		{
		}
	}
}
