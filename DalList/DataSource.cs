using DO;
using System.IO;

namespace Dal;

static internal class DataSource
{
	static DataSource()
	{
		// where do constructors go?
		s_Initialize();
	}
	readonly static Random _randomNum = new Random(1);
	
	static internal class Config
	{
		internal const int s_startingProductNumber = 100000;
		private static int s_currentProductNumber = s_startingProductNumber;
		internal static int NextProductNumber { get => s_currentProductNumber++; }

		internal const int s_startingOrderNumber = 0;
		private static int s_currentOrderNumber = s_startingOrderNumber;
		internal static int NextOrderNumber { get => s_currentOrderNumber++; }

		internal const int s_startingOrderItemNumber = 0;
		private static int s_currentOrderItemNumber = s_startingOrderItemNumber;
		internal static int NextOrderItemNumber { get => s_currentOrderItemNumber++; }


	}

	static internal Product[] Products = new Product[50];
	static internal Order[] Orders = new Order[100];
	static internal OrderItem[] OrderItems = new OrderItem[200];

	static private void s_Initialize()
	{
		ProductFiller();
		OrderFiller();
		OrderItemFiller();
	}

	static private void ProductFiller() {

		List<int> randNums = new List<int>(10);			//to track which products were already added
		int item, i = 0;								//item to add to list of products, i = index
		double price;

		do
		{
			item = _randomNum.Next(50);
			if (randNums.Contains(item))			//was the product already added?
				continue;
			randNums.Add(item);											//add product to list for future checking
			price = _randomNum.Next(1,21) + _randomNum.NextDouble();	//price is random int + random decimal = creates a double

			Product product = new Product(Config.NextProductNumber,		 //product ID
				Enums.ProductName.GetName(typeof(Enums.ProductName), item),			 //Randomly selects name from enum list of products
				Enums.Category.GetName(typeof(Enums.Category), item / 5),//Finds category in enum's list based on its location
				price, _randomNum.Next(20));							//price from above & how many currently in stock

			if (i < 2)
				product.m_inStock = 0;				//starting with 5% of products out of stock

			Products.Append(product);				//adding product to array of all products

			i++;
		} while (i < 10);				//initializing 10 products in array
	}
	
	static private void OrderFiller() {

		int num;
		
		for (int i = 0; i < 20; i++)
		{
			num = _randomNum.Next(400);
			DateTime orderDate = DateTime.Now - new TimeSpan(_randomNum.NextInt64(10L * 1000L * 1000L * 3600L * 24L * 10L));
			DateTime shipDate = DateTime.Now + new TimeSpan(_randomNum.Next(5), 4, 0, 0);
			DateTime delivDate = DateTime.Now + new TimeSpan(_randomNum.Next(5, 11), 4, 0, 0);

			string first = Enums.FirstName.GetName(typeof(Enums.FirstName), i);
			string last = Enums.LastName.GetName(typeof(Enums.LastName), i);
		
			Order order = new Order(Config.NextOrderNumber, first + last, first + last + "@gmail.com",
				num.ToString() + Enums.LastName.GetName(typeof(Enums.LastName), num / 10) +
				Enums.streetType.GetName(typeof(Enums.streetType), num / 50) , orderDate, shipDate, delivDate);

			if (i < 4) { order.m_shipDate = new DateTime(); }
			if (i < 8) { order.m_deliveryDate = new DateTime(); }

			Orders.Append(order);
		}
	}

	static private void OrderItemFiller() {

		List<int> usedOrders = new List<int>(40);           //to track which orders already have 4 orderItems
		bool confirmed;
		
		for (int i=0; i < 40; i++)							//initiating 40 OrderItems
		{
			Product productToAdd = Products[_randomNum.Next(2, 10)];	//randomly selects a product to add
			confirmed = false;
			do
			{
				Order orderToAdd = Orders[_randomNum.Next(0, 20)];		//randomly selects an order to add

				if (usedOrders.Count(o => o == orderToAdd.m_id) == 4)	//checks if there are max amount already being used
					continue;

				usedOrders.Add(orderToAdd.m_id);							//adds to list for future checks
				OrderItem orderItem = new OrderItem(Config.NextOrderItemNumber,productToAdd.m_id,
					orderToAdd.m_id, productToAdd.m_price, _randomNum.Next(1, 5));		//randomly chooses amount of product in order

				OrderItems.Append(orderItem);							//adds orderItem to array
				confirmed = true;										//ends loop

			} while (!confirmed);
		}
	}	
}
