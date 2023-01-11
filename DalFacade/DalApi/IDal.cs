namespace BlApi
{
	public interface IDal
	{
		IOrder Order { get; }
		IProduct Product { get; }
		IOrderItem OrderItem { get; }
	}
}
