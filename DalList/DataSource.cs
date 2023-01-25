using DO;

namespace Dal;

static internal class DataSource
{
	static DataSource()
	{
		s_Initialize();
	}

	readonly static Random _randomNum = new Random(1);

    ///<summary>
    /// Handles the ID's for all data types (product, order, orderItem)
    /// </summary>
    /*static internal class Config
	{
		internal const int s_startingProductNumber = 100000;
		private static int s_currentProductNumber = s_startingProductNumber;
		internal static int NextProductNumber { get => s_currentProductNumber++; }

		internal const int s_startingOrderNumber = 1;
		private static int s_currentOrderNumber = s_startingOrderNumber;
		internal static int NextOrderNumber { get => s_currentOrderNumber++; }

		internal const int s_startingOrderItemNumber = 1;
		private static int s_currentOrderItemNumber = s_startingOrderItemNumber;
		internal static int NextOrderItemNumber { get => s_currentOrderItemNumber++; }
	}*/

	static internal List<Product?> Products = new List<Product?>();
	static internal List<Order?> Orders = new List<Order?>();
	static internal List<OrderItem?> OrderItems = new List<OrderItem?>();

    ///<summary>
    /// Creates default initial entries for all arrays of product, order orderItem
    /// </summary>
    static private void s_Initialize()
	{
		ProductFiller();
		OrderFiller();
		OrderItemFiller();
	}

    ///<summary>
    /// Creates 10 inital products for the database, randomly comprised from available options
    /// </summary>
    static private void ProductFiller()
	{
		int i = 0;								//item to add to list of products
		double price;

		foreach (string name in Enum.GetNames(typeof(Enums.ProductName)))
		{
			price = Math.Round((_randomNum.Next(1, 21) + _randomNum.NextDouble()), 2);   //price is random int + random decimal = creates a double
			Product product = new Product(name, (Enums.Category)(i / 5), price, _randomNum.Next(500));
												//creates product with name & category from list and random price & quantity
			if (i % 10 == 2)
				product.m_inStock = 0;					//randomly selects products to be out of stock

			Products.Add(product);
			i++;						//for category tracking
		}
	}

    ///<summary>
    /// Creates 20 inital orders for the database, randomly comprised from available options
    /// </summary>
    static private void OrderFiller() {

		int num;
		
		for (int i = 0; i < 20; i++)						//creating 20 orders
		{
			num = _randomNum.Next(400);                     //randoming numbers for addresses

			DateTime orderDate = DateTime.Now - new TimeSpan(_randomNum.NextInt64(6048000000000, 12096000000000));		//randomizing time one week in the past
			DateTime shipDate = orderDate + new TimeSpan(_randomNum.NextInt64(3024000000000, 6048000000000));			//half week ago
			DateTime delivDate = shipDate + new TimeSpan(_randomNum.NextInt64(864000000000, 3024000000000));			//within past 3 days
			
			string first = Enums.FirstName.GetName(typeof(Enums.FirstName), i);				//getting names from enum
			string last = Enums.LastName.GetName(typeof(Enums.LastName), i);
		
			Order order = new Order(first + " " + last, first + last + "@gmail.com", num.ToString() + " " + Enums.LastName.GetName(typeof(Enums.LastName), num / 10) + " " +                //address
				Enums.streetType.GetName(typeof(Enums.streetType), num / 50),   //id, name, email
				orderDate, shipDate, delivDate);	//end of address, & dates

			if (i < 4) { order.m_shipDate = null; }										//60% shipping date is not yet set
			if (i < 8) { order.m_deliveryDate = null; }									//20% delivery date is not yet set

			Orders.Add(order);
		}
	}

    ///<summary>
    /// Creates 40 inital orderItems for the database, randomly comprised from available options
    /// </summary>
    static private void OrderItemFiller()
	{
		for (int i=0; i < 40; i++)							//initiating 40 OrderItems
		{
			OrderItem orderItem;
			if (i < 20)																		//put 1 item in each order
			{
				orderItem = new OrderItem(Products[i].Value.m_id, Orders[i].Value.m_id,
					Products[i].Value.m_price, _randomNum.Next(1, 5));                      //randomly chooses amount of product in order
				OrderItems.Add(orderItem);

				if (i % 2 == 0 && i != 0)                                                                   //for half of the items, add another product
				{
					orderItem = new OrderItem(Products[i * 2].Value.m_id, Orders[i].Value.m_id,
						Products[i * 2].Value.m_price, _randomNum.Next(1, 5));
					OrderItems.Add(orderItem);                                       //adds orderItem to list
				}
			}
			else																			//after 20 put 1 more item in each order
			{
				orderItem = new OrderItem(Products[i].Value.m_id, Orders[i - 20].Value.m_id,
					Products[i].Value.m_price, _randomNum.Next(1, 5));
				OrderItems.Add(orderItem);

				if (i % 2 == 0 && i < 26)                                                                   //for half of the items, add another product
				{
					orderItem = new OrderItem(Products[i * 2].Value.m_id, Orders[i - 20].Value.m_id,
						Products[i * 2].Value.m_price, _randomNum.Next(1, 5));                   //randomly chooses amount of product in order
					OrderItems.Add(orderItem);                                       //adds orderItem to list
				}
			}
		}
	}
}