using BlApi;

namespace BlImplementation
{
	sealed internal class Bl : IBl
	{
		public IProduct Product => new Product();

		public IOrder Order => new Order();

		public ICart Cart => new Cart();
	}
}
