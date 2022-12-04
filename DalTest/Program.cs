using System;
using DO;
using Dal;


class DalTest
{
	static private DalOrder dalOrder = new DalOrder();
	static private DalProduct dalProduct = new DalProduct();
	static private DalOrderItem dalOrderItem = new DalOrderItem();
	//static private DataSource dataSource;

	static public void Main()
	{
		int choice = -1;
		do {
			Console.WriteLine("0 - Exit\n" +
				"1 - Go to Product Menu\n" +
				"2 - Go to Order Menu\n" +
				"3 - Go to OrderItem Menu\n" +
				"Please enter your choice...");

			choice = Convert.ToInt16(Console.ReadLine());

			switch (choice)
			{
				case 0:
					Console.WriteLine("Have a good day!\n");
					break;

				case 1:
					ProductChosen();
					break;

				case 2:
					OrderChosen();
					break;

				case 3:
					OrderItemChosen();
					break;

				default:
					Console.WriteLine("Please choose a number from the above list.");
					continue;
			}

		} while (choice != 0);
		return;
	}

	static void Display(string type)
	{
		Console.WriteLine("What is the ID of the " + type + " you want to display?");
		int ID = Convert.ToInt32(Console.ReadLine());

		if (type == "Product")
			Console.WriteLine(dalProduct.ReadProduct(ID));
		else if (type == "Order")
			Console.WriteLine(dalOrder.ReadOrder(ID));
		else
			Console.WriteLine(dalOrderItem.ReadOrderItem(ID));
	}
	static void Delete(string type)
	{
		Console.WriteLine("What is the ID of the " + type + " you want to delete?");
		int ID = Convert.ToInt32(Console.ReadLine());

		if (type == "Product")
			dalProduct.DeleteProduct(ID);
		else if (type == "Order")
			dalOrder.DeleteOrder(ID);
		else
			dalOrderItem.DeleteOrderItem(ID);

		Console.WriteLine("This " + type + " has been deleted");
	}

	static void ProductChosen()
	{
		char subChoice = '-';
		do
		{
			Console.WriteLine("Product Menu\n" +
						"What would you like to do?\n" +
						"a - Create a new product\n" +
						"b - Display a product\n" +
						"c - Display all products\n" +
						"d - Update a product\n" +
						"e - Delete a  product\n" +
						"x - Return to Main Menu");
			subChoice = Console.ReadLine().First();

			switch (subChoice)
			{
				case 'x':
					Console.WriteLine("Goodbye...\n");
					break;

				case 'a':
					ProductCreate();
					break;

				case 'b':
					Display("Product");
					break;

				case 'c':
					DisplayAllProducts();
					break;

				case 'd':
					ProductUpdate();
					break;

				case 'e':
					Delete("Product");
					break;

				default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
			}

		} while (subChoice != 'x');

	}
	static void ProductCreate()
	{
		Console.WriteLine("What category does the product fall under?");
		//Console.WriteLine("What # category does the product fall under?");
		
		foreach (Enums.Category enumCategory in Enum.GetValues(typeof(Enums.Category)))
			Console.WriteLine(//enumCategory.GetHashCode() + " - "+
							  enumCategory);

		string? category = Console.ReadLine();
		//int category = Convert.ToInt32(Console.ReadLine());

		Console.WriteLine("What is the name of the product?");
		string? name = Console.ReadLine();

		Console.WriteLine("How much does the product cost?");
		double price = Convert.ToDouble(Console.ReadLine());

		Console.WriteLine("How many will be added to the inventory?");
		int inStock = Convert.ToInt16(Console.ReadLine());

		Product product = new Product(-1, name, category, price, inStock);
		Console.WriteLine("This is the ID of the product just created: " + dalProduct.CreateProduct(product));
	}

	static void DisplayAllProducts()
	{
		dalProduct.ReadAllProducts();
	}

	static void ProductUpdate()
	{
		//logic here?
	}
	static void OrderChosen()
	{
		char subChoice = '-';
		do
		{
			Console.WriteLine("Order Menu\n" +
							"What would you like to do?\n" +
							"a - Create a new order\n" +
							"b - Display an order\n" +
							"c - Display all orders\n" +
							"d - Update an order\n" +
							"e - Delete an order\n" +
							"x - Return to Main Menu");
			subChoice = Console.ReadLine().First();
			
			switch (subChoice)
			{
				case 'x':
					Console.WriteLine("Goodbye...\n");
					break;

				case 'a':
					OrderCreate();
					break;

				case 'b':
					Display("Order");
					break;

				case 'c':
					DisplayAllOrders();
					break;

				case 'd':
					OrderUpdate();
					break;

				case 'e':
					Delete("Order");
					break;

				default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
			}
		} while (subChoice != 'x');
	}

	private static void OrderCreate()
	{
		Console.WriteLine("What is the customer's name?");
		string name = Console.ReadLine();

		Console.WriteLine("What is the customer's email?");
		string email = Console.ReadLine();		//check it's a valid email?

		Console.WriteLine("What is the customer's address?");
		string address = Console.ReadLine();

		//how do we set the ship/deliv dates
		//order date set to today?

		Order order = new Order(-1, name, email, address, DateTime.Today,DateTime.Today,DateTime.Now);
		Console.WriteLine("This is the ID of the order just created: " + dalOrder.CreateOrder(order));
	}

	private static void OrderUpdate()
	{
		//??
	}

	private static void DisplayAllOrders()
	{
		dalOrder.ReadAllOrders();
	}

	static void OrderItemChosen()
	{
		char subChoice = '-';
		do
		{
			Console.WriteLine("OrderItem Menu\n" +
						"What would you like to do?\n" +
						"a - Create a new order item\n" +
						"b - Display an order item\n" +
						"c - Display all order items\n" +
						"d - Update an order item\n" +
						"e - Delete an order item\n" +
						"f - Display all items in an order\n" +
						//"g - Set items in an order\n" +
						"h - Search for an item by Product & Order ID\n" +
						"i - Set an order item based on Product & Order ID\n" + 
						"x - Return to Main Menu");
			subChoice = Console.ReadLine().First();
			
			switch (subChoice)
			{
				case 'x':
					Console.WriteLine("Goodbye...\n");
					break;

				case 'a':
					OrderItemCreate();
					break;

				case 'b':
					Display("OrderItem");
					break;

				case 'c':
					DisplayAllOrderItems();
					break;

				case 'd':
					OrderItemUpdate();
					break;

				case 'e':
					Delete("OrderItem");
					break;

				case 'f':
					DisplayAllItemsInOrder();
					break;

				case 'g':
					//set items in order
					break;

				case 'h':
					FindOrderFromProdOrdID();
					break;

				case 'i':
					SetItemFromProdOrdID();
					break;

				default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
			}
		} while (subChoice != 'x');
	}

	private static void OrderItemCreate()
	{
		Console.WriteLine("What is the product's ID?");
		int prodID = Convert.ToInt32(Console.ReadLine());
		//logic here?
		Console.WriteLine("What is the order's ID?");
		int ordID = Convert.ToInt32(Console.ReadLine());

		Console.WriteLine("What is the product's price?");	//get this from somewhere stored?
		double price = Convert.ToDouble(Console.ReadLine());

		Console.WriteLine("How many of the product are in the order?");  //get this from somewhere?
		int amount = Convert.ToInt32(Console.ReadLine());

		OrderItem orderItem = new OrderItem(-1, prodID, ordID, price, amount);
		Console.WriteLine("This is the ID of the orderItem just created: " + dalOrderItem.CreateOrderItem(orderItem));
	}

	private static void DisplayAllOrderItems()
	{
		dalOrderItem.ReadAllOrderItems();
	}

	private static void OrderItemUpdate()
	{
		//how to implement?
	}

	private static void DisplayAllItemsInOrder()
	{
		//ord ID -> find OIs
	}

	//setItemsInOrder?
	private static void FindOrderFromProdOrdID()
	{
		//ask for ids -> find OI
	}

	private static void SetItemFromProdOrdID()
	{
		//what todo here?
	}
}