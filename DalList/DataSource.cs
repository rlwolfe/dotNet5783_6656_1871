﻿using DO;
using System.IO;

namespace Dal;

static internal class DataSource
{
	static DataSource()
	{
		s_Initialize();
	}

	readonly static Random _randomNum = new Random(1);

    ///<summary>
    /// Handles the autoincrementing ID's for all data types (product, order, orderItem)
    /// </summary>
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
			price = Math.Round((_randomNum.Next(1,21) + _randomNum.NextDouble()), 2);	//price is random int + random decimal = creates a double

			Product product = new Product(Config.NextProductNumber,		 //product ID
				Enums.ProductName.GetName(typeof(Enums.ProductName), item),			 //Randomly selects name from enum list of products
				Enums.Category.GetName(typeof(Enums.Category), item / 5),//Finds category in enum's list based on its location
				price, _randomNum.Next(20));							//price from above & how many currently in stock

			if (i < 2)
				product.m_inStock = 0;              //starting with 5% of products out of stock

			Products[i] = product;				//adding product to array of all products

			i++;
		} while (i < 10);				//initializing 10 products in array
	}

    ///<summary>
    /// Creates 20 inital orders for the database, randomly comprised from available options
    /// </summary>
    static private void OrderFiller() {

		int num;
		
		for (int i = 0; i < 20; i++)						//creating 20 orders
		{
			num = _randomNum.Next(400);						//randoming numbers for addresses
			DateTime orderDate = DateTime.Now - new TimeSpan(_randomNum.NextInt64(10L * 1000L * 1000L * 3600L * 24L * 10L));
			DateTime shipDate = DateTime.Now + new TimeSpan(_randomNum.Next(5), 4, 0, 0);		//randomizing time in the past ^ and
			DateTime delivDate = DateTime.Now + new TimeSpan(_randomNum.Next(5, 11), 4, 0, 0);	//< future for the date fields

			string first = Enums.FirstName.GetName(typeof(Enums.FirstName), i);				//getting names from enum
			string last = Enums.LastName.GetName(typeof(Enums.LastName), i);
		
			Order order = new Order(Config.NextOrderNumber, first + " " + last, first + last + "@gmail.com",	//id, name, email
				num.ToString() + Enums.LastName.GetName(typeof(Enums.LastName), num / 10) +						//address
				Enums.streetType.GetName(typeof(Enums.streetType), num / 50) , orderDate, shipDate, delivDate);	//end of address, & dates

			if (i < 4) { order.m_shipDate = DateTime.MinValue; }										//60% shipping date is not yet set
			if (i < 8) { order.m_deliveryDate = DateTime.MinValue; }									//20% delivery date is not yet set

			Orders[i] = order;
		}
	}

    ///<summary>
    /// Creates 40 inital orderItems for the database, randomly comprised from available options
    /// </summary>
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
					orderToAdd.m_id, productToAdd.m_price, _randomNum.Next(1, 5));      //randomly chooses amount of product in order

				OrderItems[i] = orderItem;							//adds orderItem to array
				confirmed = true;										//ends loop

			} while (!confirmed);
		}
	}	
}
