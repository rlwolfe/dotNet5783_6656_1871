using DO;

namespace Dal;

static internal class DataSource
{
	static DataSource()
	{

		/// TODO add constructor
		/// config class (end of page 8)
		///		class config{static productCount = 0
		///		/// product(){ ID=Config::prodcount++;

		s_Initialize();
	}
	readonly static Random _randomNum = new Random();

	static private void s_Initialize()
	{ //loads information randomly into the arrays
		
		ProductFiller(); }

	//TODO get rid of 'Dal' in array names/types


	static internal Order[] Orders = new Order[100];
	private void OrderFiller(Order order) { Orders.Append(order); }

	internal OrderItem[] OrderItems = new OrderItem[200];

	private void OrderItemFiller(OrderItem orderItem) { OrderItems.Append(orderItem); }

	internal Product[] Products = new Product[50];

	private void ProductFiller(Product product) { Products.Append(product); }
}
