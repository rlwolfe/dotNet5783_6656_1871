using System;
using DO;
using Dal;
using System.Security.Cryptography.X509Certificates;
using BlApi;

///<summary>
/// Tests for the functionality of all data types, with their CRUD functions
/// </summary>
class DalTest
{
	static IDal dalList = new DalList();

    ///<summary>
    /// Main Menu
    /// </summary>
    static public void Main()
	{
		int choice;
		do {
			Console.WriteLine("0 - Exit\n" +
				"1 - Go to Product Menu\n" +
				"2 - Go to Order Menu\n" +
				"3 - Go to OrderItem Menu\n" +
				"Please enter your choice...");

			choice = Convert.ToInt16(Console.ReadLine());
			try
			{
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
			} catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			} while (choice != 0);
		return;
	}

	static void Display(string type)
	{
		Console.WriteLine("What is the ID of the " + type + " you want to display?");
		int ID = Convert.ToInt32(Console.ReadLine());

		if (type == "Product")
			Console.WriteLine(dalList.Product.Read(ID).ToString());
		else if (type == "Order")
			Console.WriteLine(dalList.Order.Read(ID).ToString());
		else
			Console.WriteLine(dalList.OrderItem.Read(ID).ToString());
	}
	static void Delete(string type)
	{
		Console.WriteLine("What is the ID of the " + type + " you want to delete?");
		int ID = Convert.ToInt32(Console.ReadLine());

		if (type == "Product")
			dalList.Product.Delete(ID);
		else if (type == "Order")
			dalList.Order.Delete(ID);
		else
			dalList.OrderItem.Delete(ID);

		Console.WriteLine("This " + type + " has been deleted");
	}

    ///<summary>
    /// Product SubMenu
    /// </summary>
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
    ///<summary>
    /// User Creates new Product
    /// </summary>
    static void ProductCreate()
	{
		Console.WriteLine("What category does the product fall under?");
		//Console.WriteLine("What # category does the product fall under?");
		
		foreach (Enums.Category enumCategory in Enum.GetValues(typeof(Enums.Category)))
			Console.WriteLine(enumCategory.GetHashCode() + " - " + enumCategory);

		int catNum = Convert.ToInt16(Console.ReadLine());
		Enums.Category category = (Enums.Category)catNum;

		Console.WriteLine("What is the name of the product?");
		string? name = Console.ReadLine();

		Console.WriteLine("How much does the product cost?");
		double price = Convert.ToDouble(Console.ReadLine());

		Console.WriteLine("How many will be added to the inventory?");
		int inStock = Convert.ToInt32(Console.ReadLine());

		Product product = new Product(name, category, price, inStock);
		Console.WriteLine("This is the ID of the product just created: " + dalList.Product.Create(product));
	}
    ///<summary>
    /// User Updates a Product
    /// </summary>
    static void ProductUpdate()
	{
		Console.WriteLine("What is the ID of the product you want to update?");
		int ID = Convert.ToInt32(Console.ReadLine());

		Product product = dalList.Product.Read(ID);
		Console.WriteLine(product);

		Console.WriteLine("What field would you like to update?");
		int field = -1;

		do
		{
			Console.WriteLine("Fields available for update:\n" +
						"1 - Name\n" +
						"2 - Category\n" +
						"3 - Price\n" +
						"4 - How many are in stock\n" +
						"0 - Finalize the update");
			field = Convert.ToInt32(Console.ReadLine());

			switch (field)
			{
				case 0:
					Console.WriteLine("Thank you\n");
					break;

				case 1:
					Console.WriteLine("What is the new name of the product?");
					string? temp = Console.ReadLine();
					if (temp != null)
						product.m_name = temp;
					break;

				case 2:
					Console.WriteLine("What category number does the product fall under?");
					foreach (Enums.Category enumCategory in Enum.GetValues(typeof(Enums.Category)))
						Console.WriteLine(enumCategory.GetHashCode() + " - " + enumCategory);

					if (int.TryParse(Console.ReadLine(), out int intTemp))
						product.m_category = (Enums.Category)Enum.ToObject(typeof(DO.Enums.Category), intTemp);
					else
						Console.WriteLine("invalid category");
					break;

				case 3:
					Console.WriteLine("How much does the product cost?");
					if (double.TryParse(Console.ReadLine(), out double tempDub))
						product.m_price = tempDub;
					break;

				case 4:
					Console.WriteLine("How many of the product are stocked?");
					if (int.TryParse(Console.ReadLine(), out int tempInt))
						product.m_inStock = tempInt;
					break;

				default:
					Console.WriteLine("Please choose a valid option.");
					break;
			}
		} while (field != 0);

		dalList.Product.Update(product);
		Console.WriteLine("This product has been updated");
	}
    static void DisplayAllProducts()
    {
        foreach (Product product in dalList.Product.ReadAll())
        {
            if (product.m_id != 0)
                Console.WriteLine(product);
        }
    }

    ///<summary>
    /// Order SubMenu
    /// </summary>
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
    ///<summary>
    /// User Creates new Order
    /// </summary>
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

		Order order = new Order(name, email, address, DateTime.Today, DateTime.Today, DateTime.Now);
		Console.WriteLine("This is the ID of the order just created: " + dalList.Order.Create(order));
	}
    ///<summary>
    /// User Updates an Order
    /// </summary>
    private static void OrderUpdate()
	{
		Console.WriteLine("What is the ID of the order you want to update?");
		int ID = Convert.ToInt32(Console.ReadLine());
		string temp;

		Order order= dalList.Order.Read(ID);
		Console.WriteLine(order);

		Console.WriteLine("What field would you like to update?");
		int field = -1;
		do
		{
			Console.WriteLine("Fields available for update:\n" +
						"1 - Customer's Name\n" +
						"2 - Customer's Email\n" +
						"3 - Customer's Address\n" +
						"4 - Order Date\n" +
						"4 - Shipping Date\n" +
						"4 - Delivery Date\n" +
						"0 - Finalize the Update");
			field = Convert.ToInt32(Console.ReadLine());

			switch (field)
			{
				case 0:
					Console.WriteLine("Thank you\n");
					break;

				case 1:
					Console.WriteLine("What is the customer's name?");
					temp = Console.ReadLine();
					if (temp != null)
						order.m_customerName= temp;
					break;

				case 2:
					Console.WriteLine("What is the customer's email?");
					temp = Console.ReadLine();
					if (temp != null)
						order.m_customerEmail = temp;
					break;

				case 3:
					Console.WriteLine("What is the customer's address?");
					temp = Console.ReadLine();
					if (temp != null)
						order.m_customerAddress = temp;
					break;

				case 4:
					Console.WriteLine("What is the order date? (please input data in this format dd/mm/yyyy)");
					order.m_orderDate = DateTime.ParseExact(Console.ReadLine(), "dd/mm/yyyy", System.Globalization.CultureInfo.InvariantCulture);
					break;

				case 5:
					Console.WriteLine("What is the shipping date? (please input data in this format dd/mm/yyyy)");
					order.m_shipDate = DateTime.ParseExact(Console.ReadLine(), "dd/mm/yyyy", System.Globalization.CultureInfo.InvariantCulture);
					break;

				case 6:
					Console.WriteLine("What is the delivery date? (please input data in this format dd/mm/yyyy)");
					order.m_deliveryDate = DateTime.ParseExact(Console.ReadLine(), "dd/mm/yyyy", System.Globalization.CultureInfo.InvariantCulture);
					break;

				default:
					Console.WriteLine("Please choose a valid option.");
					break;
			}
		} while (field != 0);

		dalList.Order.Update(order);
		Console.WriteLine("This order has been updated");
	}
	private static void DisplayAllOrders()
	{
		foreach (Order order in dalList.Order.ReadAll())
		{
			if (order.m_id != 0)
				Console.WriteLine(order);
		}
	}

    ///<summary>
    /// OrderItem SubMenu
    /// </summary>
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

				/*case 'g':
					Console.WriteLine("This doesn't work yet");
					SetAllItemsInOrder();
					//set items in an order - he'll get back to us
					break;*/

				case 'h':
					FindOrderItemFromProdOrdID();
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
    ///<summary>
    /// User Creates new OrderItem
    /// </summary>
    private static void OrderItemCreate()
	{
		Console.WriteLine("What is the product's ID?");
		int prodID = Convert.ToInt32(Console.ReadLine());

		Console.WriteLine("What is the order's ID?");
		int ordID = Convert.ToInt32(Console.ReadLine());

		double price = dalList.Product.Read(prodID).m_price;

		Console.WriteLine("How many of the product are in the order?");
		int amount = Convert.ToInt32(Console.ReadLine());

		OrderItem orderItem = new OrderItem(prodID, ordID, price, amount);
		Console.WriteLine("This is the ID of the orderItem just created: " + dalList.OrderItem.Create(orderItem));
	}
    ///<summary>
    /// User Updates an OrderItem
    /// </summary>
    private static void OrderItemUpdate()
	{
		Console.WriteLine("What is the ID of the order item you want to update?");
		int temp, ID = Convert.ToInt32(Console.ReadLine());

		OrderItem orderItem = dalList.OrderItem.Read(ID);
		Console.WriteLine(orderItem);

		Console.WriteLine("What field would you like to update?");
		int field = -1;
		do
		{
			Console.WriteLine("Fields available for update:\n" +
						"1 - Product's ID\n" +
						"2 - Order's ID\n" +
						"3 - Price\n" +
						"4 - Amount in order\n" +
						"0 - Finalize the update");
			field = Convert.ToInt32(Console.ReadLine());

			switch (field)
			{
				case 0:
					Console.WriteLine("Thank you\n");
					break;

				case 1:
					Console.WriteLine("What is the ID of the product?");
					if (int.TryParse(Console.ReadLine(), out temp))
						orderItem.m_productID = temp;
					break;

				case 2:
					Console.WriteLine("What is the ID of the order?");
					if (int.TryParse(Console.ReadLine(), out temp))
						orderItem.m_orderID = temp;
					break;

				case 3:
					Console.WriteLine("How much does the order item cost?");
					if (double.TryParse(Console.ReadLine(), out double tempDub))
						orderItem.m_price = tempDub;
					break;

				case 4:
					Console.WriteLine("How many of the product are in the order?");
					if (int.TryParse(Console.ReadLine(), out temp))
						orderItem.m_amount = temp;
					break;

				default:
					Console.WriteLine("Please choose a valid option.");
					break;
			}
		} while (field != 0);

		dalList.OrderItem.Update(orderItem);
		Console.WriteLine("This order item has been updated");
	}

	///<summary>
	/// Displays all OrderItems
	/// </summary>
	private static void DisplayAllOrderItems()
    {
        foreach (OrderItem orderItem in dalList.OrderItem.ReadAll())
        {
            if (orderItem.m_id != 0)
                Console.WriteLine(orderItem);
        }
    }

    ///<summary>
    /// Displays all OrderItems in a given Order
    /// </summary>
    private static void DisplayAllItemsInOrder()
	{
		Console.WriteLine("What is the ID of the order?");

		if (int.TryParse(Console.ReadLine(), out int orderID))
			foreach (OrderItem orderItem in dalList.OrderItem.GetItemsInOrder(orderID))
				Console.WriteLine(orderItem);

	}

    ///<summary>
    /// Insufficient data given for use case, return variable or parameters of function
    ///- on hold while professor gets more info
    /// </summary>
    private static void SetAllItemsInOrder()
	{//he'll get back to us
		Console.WriteLine("What is the ID of the order?");
		
		if (int.TryParse(Console.ReadLine(), out int orderID))	
			dalList.OrderItem.SetItemsInOrder(orderID);
	}

    ///<summary>
    /// Read function for OrderItems when given Product ID and OrderID (by user)
    /// </summary>
    private static void FindOrderItemFromProdOrdID()
	{
		Console.WriteLine("Please enter the product ID followed by the order ID on the next line");

		if (int.TryParse(Console.ReadLine(), out int productID) && int.TryParse(Console.ReadLine(), out int orderID))
			Console.WriteLine(dalList.OrderItem.GetOrderItemWithProdAndOrderID(productID, orderID));
	}

    ///<summary>
    /// Update function for OrderItems when given Product ID and OrderID (by user)
    /// </summary>
    private static void SetItemFromProdOrdID()
	{
		Console.WriteLine("Please enter the product ID followed by the order ID on the next line");

		if (int.TryParse(Console.ReadLine(), out int productID) && int.TryParse(Console.ReadLine(), out int orderID))
			dalList.OrderItem.SetOrderItemWithProdAndOrderID(productID, orderID);
	}
}