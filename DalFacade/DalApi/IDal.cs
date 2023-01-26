//using BlApi;

namespace DalApi
{
    public interface IDal
    {
        IOrder Order { get; }
        IProduct Product { get; }
        IOrderItem OrderItem { get; }
    }
}
