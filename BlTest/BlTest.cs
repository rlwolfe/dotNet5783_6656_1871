using BlApi;
using BlImplementation;
using BO;
using Dal;
using DO;
using System.Diagnostics;

namespace BlTest;
class BlTest
{
    static IBl bl = new Bl();
    //static BO.Cart cart = new BO.Cart();  //can't do it in main (w/o static) as is not recognised in customer menu
										    //can't do here w/o static keyword - but still not sure if this is correct choice
										   //can put in customer menu w/o static - poss solution?
    static void Main(string[] args)
    {
        
        int choice;
		do
		{
			Console.WriteLine("0 - Exit\n" +
				"Who are you?\n" +
				"1 - I am a manager\n" +
				"2 - I am a customer");

			choice = Convert.ToInt16(Console.ReadLine());
			try
			{
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
				Console.WriteLine(e.Message);
			}

		} while (choice != 0);
		return;
	}

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
						"d - Delete a product\n" +                  //deal with data source
						"e - Update a  product\n" +                 //update needs id
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
                    //DisplayAllProductsManagaer();
                    foreach (BO.ProductForList product in bl.Product.ManagerListRequest())
                    {
                        if (product.m_id != 0)
                            Console.WriteLine(product);
                    }
                    break;

				case 'b':
                    Console.WriteLine("What is the ID of the product you want to display?");
                    int ID = Convert.ToInt32(Console.ReadLine());
					//try catch?
                    Console.WriteLine(bl.Product.ManagerRequest(ID).ToString());
                    break;

				case 'c':
					ProductCreate();
                    break;

				case 'd':
                    Console.WriteLine("What is the ID of the product you want to delete?");
                    ID = Convert.ToInt32(Console.ReadLine());
                    bl.Product.Delete(ID);
					break;

				case 'e':
					ProductUpdate();
                    break;

				case 'f':
                    foreach (BO.OrderForList order in bl.Order.ReadAll())
                    {
                        if (order.m_id != 0)
                            Console.WriteLine(order);
                    }
                    break;

				case 'g':
                    Console.WriteLine("What is the ID of the order you want to display?");
                    ID = Convert.ToInt32(Console.ReadLine());
                    try
                    {
                        Console.WriteLine(bl.Order.Read(ID).ToString());
                    }
                    catch (BO.InputIsInvalidException exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Something went wrong - bl reading order");
                        Console.WriteLine(exc.Message);
                    }
                    break;

				case 'h':

					break;

				case 'i':

					break;

				case 'j':
                    Console.WriteLine("What is the ID of the order you want to track?");
                    ID = Convert.ToInt32(Console.ReadLine());
					try
					{
						bl.Order.OrderTracker(ID);
					}
                    catch (BO.dataLayerIdNotFoundException exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                    catch (BO.blGeneralException)
                    {
                        Console.WriteLine("blGeneralException - bl order tracking");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Some other problem: " + exc.Message); //maybe?
                    }
					break;

					default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
					}

		} while (subChoice != 'x');

	}



    static void ProductCreate()
    {
        BO.Product product = new BO.Product();
        Console.WriteLine("What category does the product fall under?");
        //Console.WriteLine("What # category does the product fall under?");

        foreach (BO.Enums.Category enumCategory in Enum.GetValues(typeof(BO.Enums.Category)))
            Console.WriteLine(enumCategory.GetHashCode() + " - " + enumCategory);

        int catNum = Convert.ToInt16(Console.ReadLine());
        product.m_category = (BO.Enums.Category)catNum;

        Console.WriteLine("What is the name of the product?");
        product.m_name = Console.ReadLine();

        Console.WriteLine("How much does the product cost?");
        product.m_price = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("How many will be added to the inventory?");
        product.m_inStock = Convert.ToInt32(Console.ReadLine());

        try
        {
            bl.Product.Create(product);
        }
        catch (BO.dataLayerIdAlreadyExistsException)
        {
            Console.WriteLine("the product with id: " + product.m_id + "was not created"); //maybe?
        }
		catch (BO.blGeneralException)
		{
			Console.WriteLine("blGeneralException - bl product creation");
		}
        catch (Exception exc)
        {
            Console.WriteLine("Some other problem: " + exc.Message); //maybe?
        }
        Console.WriteLine("This is the ID of the product just created: " + product.m_id);
    }
    static void ProductUpdate()
    {
        BO.Product product = new BO.Product();
        Console.WriteLine("What category does the product fall under?");
        //Console.WriteLine("What # category does the product fall under?");

        foreach (BO.Enums.Category enumCategory in Enum.GetValues(typeof(BO.Enums.Category)))
            Console.WriteLine(enumCategory.GetHashCode() + " - " + enumCategory);

        int catNum = Convert.ToInt16(Console.ReadLine());
        product.m_category = (BO.Enums.Category)catNum;
                                                                        //ask for id
        Console.WriteLine("What is the name of the product?");
        product.m_name = Console.ReadLine();

        Console.WriteLine("How much does the product cost?");
        product.m_price = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("How many will be added to the inventory?");
        product.m_inStock = Convert.ToInt32(Console.ReadLine());

		try
		{
			bl.Product.Update(product);
		}
		catch(BO.dataLayerIdNotFoundException exc)
		{
            Console.WriteLine("the id: " + product.m_id + "was not found");
        }
        catch (BO.blGeneralException)
        {
            Console.WriteLine("blGeneralException - bl product update");
        }
        catch (Exception exc)
        {
            Console.WriteLine("Some other problem: " + exc.Message); //maybe?
        }
    }

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
						"c - Add product to cart\n" +                       //problematic
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
                    //DisplayAllProductsCustomer();
                    foreach (BO.Product product in bl.Product.CatalogRequest())
                    {
                        if (product.m_id != 0)
                            Console.WriteLine(product);
                    }
                    break;

				case 'b':
                    Console.WriteLine("What is the ID of the product you want to display?");
                    int ID = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(bl.Product.CustomerRequest(ID).ToString());
                    break;

				case 'c':
                    Console.WriteLine("What is the ID of the product you want to add to the cart?");
                    ID = Convert.ToInt32(Console.ReadLine());
					try
					{
						bl.Cart.AddToCart(cart2, ID); //see nores on cart at top
					}
                    catch (BO.dataLayerIdNotFoundException exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                    catch (BO.blGeneralException)
                    {
                        Console.WriteLine("blGeneralException - adding product to cart");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Some other problem: " + exc.Message); //maybe?
                    }
                    break;

				case 'd':
                    Console.WriteLine("What is the ID of the product you want to change in the cart?");
                    ID = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("What is the new amount?");
                    int amount = Convert.ToInt32(Console.ReadLine());
                    try
                    {
                        bl.Cart.Update(cart2, ID, amount);
                    }
                    catch(BO.dataLayerIdNotFoundException exc) 
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
                    string? customerEmail = Console.ReadLine();      //check it's a valid email?

                    Console.WriteLine("What is the customer's address?");
                    string? customerAddress = Console.ReadLine();

                    try
                    {
                        bl.Cart.PlaceOrder(cart2, customerName, customerEmail, customerAddress);
                    }
                    catch(BO.InputIsInvalidException exc)
                    {
                        Console.WriteLine(exc.Message); 
                    }
                    catch (BO.dataLayerIdAlreadyExistsException exc)
                    {
                        Console.WriteLine(exc.Message); //maybe?
                    }
                    catch(BO.dataLayerIdNotFoundException exc)
                    {
                        Console.WriteLine(exc.Message); 
                    }
                    catch (BO.blGeneralException)
                    {
                        Console.WriteLine("blGeneralException - bl order placement");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Some other problem: " + exc.Message); //maybe?
                    }
                    break;

				case 'f':
                    Console.WriteLine("What is the ID of the order you want to display?");
                    ID = Convert.ToInt32(Console.ReadLine());
                    //try catch?
                    try
                    {
                        Console.WriteLine(bl.Order.Read(ID).ToString());
                    }
                    catch(BO.InputIsInvalidException exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                    catch(Exception exc)
                    {
                        Console.WriteLine("Something went wrong - bl reading order");
                        Console.WriteLine(exc.Message);
                    }
                    break;
				default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
			}

		} while (subChoice != 'x');
	}

    /*  static void DisplayAllProductsCustomer()
      {
          foreach (BO.Product product in bl.Product.CatalogRequest())
          {
              if (product.m_id != 0)
                  Console.WriteLine(product);
          }
      }*/
}
