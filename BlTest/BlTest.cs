using BlApi;
using BlImplementation;

namespace BlTest;
class BlTest
{
	static IBl bl = new Bl();
	/// <summary>
	/// Main runs a do/while loop until the user wishes to exit the program
	/// </summary>
	static void Main(string[] args)
	{
		int choice;
		do
		{
			Console.WriteLine("Main Menu\n" +
				"Who are you?\n" +
				"1 - I am a manager\n" +
				"2 - I am a customer\n" +
				"0 - Exit");

			try
			{
				choice = Convert.ToInt32(Console.ReadLine());
				switch (choice)
				{
					case 0:
						Console.WriteLine("Have a good day!\n");
						break;

					case 1:
						ManagerChosen();
						break;

					case 2:
						CustomerChosen();
						break;
					default:
						Console.WriteLine("Please choose a number from the above list.");
						continue;
				}
			}
			catch (Exception e)
			{
				choice = 3;
				Console.WriteLine(e.Message);
			}

		} while (choice != 0);
		return;
	}

	/// <summary>
	/// manager capabilities according to what actions may need to be done
	/// </summary>
	static void ManagerChosen()
	{
		char subChoice = '-';
		do
		{
			Console.WriteLine("Manager Menu\n" +
						"What would you like to do?\n" +
						"a - View list of products\n" +
						"b - Single product details\n" +
						"c - Add a product\n" +
						"d - Delete a product\n" +
						"e - Update a  product\n" +
						"f - View list of orders\n" +
						"g - Single order details\n" +
						"h - Update shipped order\n" +
						"i - Update delivered order\n" +
						"j - Track an order\n" +
						"x - Return to Main Menu");
			subChoice = Console.ReadLine().First();

			switch (subChoice)
			{
				case 'x':
					Console.WriteLine("Goodbye...\n");
					break;

				case 'a':
					foreach (BO.ProductForList product in bl.Product.ManagerListRequest())          //runs through all products and prints them without amount variable
					{
						if (product.m_id != 0)
							Console.WriteLine(product);
					}
					break;

				case 'b':
					Console.WriteLine("What is the ID of the product you want to display?");
					int ID = Convert.ToInt32(Console.ReadLine());
					Console.WriteLine(bl.Product.ManagerRequest(ID).ToString());                    //sends request to show manager the product details
					break;

				case 'c':
					ProductCreate();
					break;

				case 'd':
					Console.WriteLine("What is the ID of the product you want to delete?");
					ID = Convert.ToInt32(Console.ReadLine());
					bl.Product.Delete(ID);                                                          //deletes product, if it doesn't exist in anyones cart
					break;

				case 'e':
					ProductUpdate();
					break;

				case 'f':
					foreach (BO.OrderForList order in bl.Order.ReadAll())
					{
						if (order.m_id != 0)
							Console.WriteLine(order);                                               //displays all current orders
					}
					break;

				case 'g':
					Console.WriteLine("What is the ID of the order you want to display?");
					ID = Convert.ToInt32(Console.ReadLine());
					try
					{
						Console.WriteLine(bl.Order.Read(ID).ToString());                            //if order exists, it displays that order
					}
					catch (BO.InputIsInvalidException exc)
					{
						Console.WriteLine(exc.Message);
					}
					break;

				case 'h':
					Console.WriteLine("What is the ID of the order you want to mark as shipped?");
					ID = Convert.ToInt32(Console.ReadLine());
					bl.Order.UpdateOrderStatus(ID, BO.Enums.OrderStatus.Shipped);                           //updates order as shipped
					break;

				case 'i':
					Console.WriteLine("What is the ID of the order you want to mark as delivered?");
					ID = Convert.ToInt32(Console.ReadLine());
					bl.Order.UpdateOrderStatus(ID, BO.Enums.OrderStatus.Delivered);                           //updates order as delivered
					break;

				case 'j':
					Console.WriteLine("What is the ID of the order you want to track?");
					ID = Convert.ToInt32(Console.ReadLine());
					Console.WriteLine(bl.Order.OrderTracker(ID));                                   //sets tracking pairs for package tracking

					break;

				default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
			}

		} while (subChoice != 'x');

	}
	/// <summary>
	/// manager can create a product with this method
	/// </summary>
	static void ProductCreate()
	{
		BO.Product product = new BO.Product();
		Console.WriteLine("What category # does the product fall under?");

		foreach (BO.Enums.Category enumCategory in Enum.GetValues(typeof(BO.Enums.Category)))
			Console.WriteLine(enumCategory.GetHashCode() + " - " + enumCategory);

		int catNum = Convert.ToInt32(Console.ReadLine());
		product.m_category = (BO.Enums.Category)catNum;

		Console.WriteLine("What is the name of the product?");
		product.m_name = Console.ReadLine();

		Console.WriteLine("How much does the product cost?");
		product.m_price = Convert.ToDouble(Console.ReadLine());

		Console.WriteLine("How many will be added to the inventory?");
		product.m_inStock = Convert.ToInt32(Console.ReadLine());
		int id = -1;
		try
		{
			id = bl.Product.Create(product);                                                //creates a product here
			Console.WriteLine("This is the ID of the product just created: " + id);
		}
		catch (BO.dataLayerIdAlreadyExistsException)
		{
			Console.WriteLine("the product with id: " + id + "was not created");
		}
		catch (BO.blGeneralException)
		{
			Console.WriteLine("blGeneralException - bl product creation");
		}
		catch (Exception exc)
		{
			Console.WriteLine("Some other problem: " + exc.Message);
		}
	}
	/// <summary>
	/// manager can update a product here
	/// </summary>
	static void ProductUpdate()
	{
		Console.WriteLine("What is the ID of the product you want to update?");
		int ID = Convert.ToInt32(Console.ReadLine());

		BO.Product product = bl.Product.ManagerRequest(ID);
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
					foreach (BO.Enums.Category enumCategory in Enum.GetValues(typeof(BO.Enums.Category)))
						Console.WriteLine(enumCategory.GetHashCode() + " - " + enumCategory);

					if (int.TryParse(Console.ReadLine(), out int intTemp))
						product.m_category = (BO.Enums.Category)Enum.ToObject(typeof(DO.Enums.Category), intTemp);
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
		try
		{
			bl.Product.Update(product);                                                             //updates here
		}
		catch (BO.dataLayerIdNotFoundException exc)
		{
			Console.WriteLine("the id: " + product.m_id + "was not found");
		}
		catch (BO.blGeneralException)
		{
			Console.WriteLine("blGeneralException - bl product update");
		}
		catch (Exception exc)
		{
			Console.WriteLine("Some other problem: " + exc.Message);
		}
		Console.WriteLine("This product has been updated");
	}
	/// <summary>
	/// customer capabilities for what a customer needs to shop
	/// </summary>
	static void CustomerChosen()
	{
		BO.Cart cart2 = new BO.Cart();
		char subChoice = '-';
		do
		{
			Console.WriteLine("Customer Menu\n" +
						"What would you like to do?\n" +
						"a - View list of products\n" +
						"b - Single product details\n" +
						"c - Add product to cart\n" +
						"d - Update quantity or product in cart\n" +
						"e - Place the order\n" +
						"f - View order details\n" +
						"x - Return to Main Menu");
			subChoice = Console.ReadLine().First();

			switch (subChoice)
			{
				case 'x':
					Console.WriteLine("Goodbye...\n");
					break;

				case 'a':
					foreach (BO.Product product in bl.Product.CatalogRequest())
					{
						if (product.m_id != 0)
							Console.WriteLine(product);                                                         //prints list of all products in shop
					}
					break;

				case 'b':
					Console.WriteLine("What is the ID of the product you want to display?");
					int ID = Convert.ToInt32(Console.ReadLine());
					Console.WriteLine(bl.Product.CustomerRequest(ID).ToString());                               //gets details of specific product
					break;

				case 'c':
					Console.WriteLine("What is the ID of the product you want to add to the cart?");
					ID = Convert.ToInt32(Console.ReadLine());
					try
					{
						bl.Cart.AddToCart(cart2, ID);                                                           //adds product to cart
					}
					catch (BO.dataLayerIdNotFoundException exc)
					{
						Console.WriteLine(exc.Message);
					}
					break;

				case 'd':
					Console.WriteLine("What is the ID of the product you want to update in the cart?");
					ID = Convert.ToInt32(Console.ReadLine());
					Console.WriteLine("What is the new amount?");
					int amount = Convert.ToInt32(Console.ReadLine());
					try
					{
						bl.Cart.Update(cart2, ID, amount);                                                              //product amount updates in cart
					}
					catch (BO.dataLayerIdNotFoundException exc)
					{
						Console.WriteLine(exc.Message);
					}
					catch (BO.blGeneralException)
					{
						Console.WriteLine("blGeneralException - updating product in cart");
					}
					catch (Exception exc)
					{
						Console.WriteLine("Some other problem: " + exc.Message);
					}
					break;

				case 'e':

					Console.WriteLine("What is the customer's name?");
					string? customerName = Console.ReadLine();

					Console.WriteLine("What is the customer's email?");
					string? customerEmail = Console.ReadLine();

					Console.WriteLine("What is the customer's address?");
					string? customerAddress = Console.ReadLine();

					try
					{
						bl.Cart.PlaceOrder(cart2, customerName, customerEmail, customerAddress);                    //placing an order
						Console.WriteLine("The order was just placed\nThank you for shopping with us!");
						subChoice = 'x';
					}
					catch (BO.InputIsInvalidException exc)
					{
						Console.WriteLine(exc.Message);
					}
					catch (BO.dataLayerIdAlreadyExistsException exc)
					{
						Console.WriteLine(exc.Message);
					}
					catch (BO.dataLayerIdNotFoundException exc)
					{
						Console.WriteLine(exc.Message);
					}
					catch (BO.blGeneralException)
					{
						Console.WriteLine("blGeneralException - bl order placement");
					}
					catch (Exception exc)
					{
						Console.WriteLine("Some other problem: " + exc.Message);
					}
					break;

				case 'f':
					Console.WriteLine("What is the ID of the order you want to display?");
					ID = Convert.ToInt32(Console.ReadLine());
					Console.WriteLine(bl.Order.Read(ID).ToString());                                    //display an already placed order
					break;
				default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
			}

		} while (subChoice != 'x');
	}
}
