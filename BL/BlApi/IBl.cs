namespace BlApi
{
	public interface IBl
	{
		public IProduct Product { get; }
		public IOrder Order { get; }
		public IOrderItem OrderItem { get; }
	}
}
