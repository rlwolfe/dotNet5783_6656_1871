using BlApi;
using System.Text.RegularExpressions;

namespace BlImplementation
{
	internal class Order : IOrder
	{
		static private IDal? dal = DalApi.Factory.Get();
		static public List<BO.Order> Orders = new List<BO.Order>();				//holds orders containing tracking status
		
		/// <summary>
		/// creates a new order with data from the user
		/// this method is used by the customers
		/// </summary>
		/// <param name="order">from the BO layer</param>
		/// <returns>id of newly created product</returns>
		/// <exception cref="BO.dataLayerIdAlreadyExistsException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public int Create(BO.Order order)
		{
			if (Orders.Count == 0)								//if orders haven't been pulled up from the data layer yet, do it now
				fillOrders();
			InputValidation(order);

			DO.Order ord = new DO.Order(order.m_customerName, order.m_customerEmail, order.m_customerAddress, order.m_orderDate,
			order.m_shipDate, order.m_deliveryDate);
			try
			{
				dal.Order.Create(ord);
			}
			catch (DO.idAlreadyExistsException exc)
			{
				Console.WriteLine(ord.m_id);
				throw new BO.dataLayerIdAlreadyExistsException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem");
				throw new BO.blGeneralException();
			}
			Orders.Add(order);
			return ord.m_id;
		}

		/// <summary>
		/// get information about order from data layer to display
		/// </summary>
		/// <param name="id"></param>
		/// <returns>instance of requested order</returns>
		/// <exception cref="BO.dataLayerEntityNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public BO.Order Read(int id)
		{
			BO.Order order = new BO.Order();
			try
			{
				if (id > 0)												//if ID is valid then get it
					order = Orders.Find(x => x.m_id == id);
				else
					throw new BO.InputIsInvalidException("ID");
			}
			catch (BO.InputIsInvalidException)
			{
				Console.WriteLine(id);
			}
			if (order != null)											//and return it here
				return order;

			order = new BO.Order();										//reset order so it's not null
			try
			{
				DO.Order doOrd = dal.Order.Read(id);					//fill BO order with data layer order's information
				order.m_id = doOrd.m_id;
				order.m_customerName = doOrd.m_customerName;
				order.m_customerEmail = doOrd.m_customerEmail;
				order.m_customerAddress = doOrd.m_customerAddress;
				order.m_orderDate = doOrd.m_orderDate;
				order.m_shipDate = doOrd.m_shipDate;
				order.m_deliveryDate = doOrd.m_deliveryDate;
				order.m_paymentDate = doOrd.m_orderDate;

				if (order.m_orderDate != null && order.m_orderDate <= DateTime.Now)
					if (order.m_shipDate != null && order.m_shipDate <= DateTime.Now)
						if (order.m_deliveryDate != null && order.m_deliveryDate <= DateTime.Now)
							order.m_status = BO.Enums.OrderStatus.Delivered;                      //past delivery, shipped and ordered
						else
							order.m_status = BO.Enums.OrderStatus.Shipped;                        //not past delivery, but past order and shipped dates
					else
						order.m_status = BO.Enums.OrderStatus.Ordered;                            //not past shipped, but past ordered
				else
					order.m_status = BO.Enums.OrderStatus.None;                                   //not past shipped, ordered or delivered



				order.m_totalPrice = 0;
				order.m_items = new();
				foreach (DO.OrderItem doItem in dal.OrderItem.ReadAllFiltered())						//find and add orderItems
				{
					BO.OrderItem boItem = new();
					if (doItem.m_orderID == id)
					{
						boItem.m_id = doItem.m_id;
						boItem.m_productID = doItem.m_productID;
						boItem.m_amount = doItem.m_amount;
						boItem.m_price = doItem.m_price;
					}
					else
						continue;

					order.m_totalPrice += (boItem.m_price * boItem.m_amount);					//set price accordingly
					order.m_items.Add(boItem);
				}
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(id);
				throw new BO.dataLayerEntityNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem");
				throw new BO.blGeneralException();
			}
			return order;
		}

		/// <summary>
		/// get all orders for manager to see
		/// </summary>
		/// <returns>list of shortened orders' data</returns>
		public IEnumerable<BO.OrderForList> ReadAll()
		{
			if (Orders.Count == 0)                              //if orders haven't been pulled up from the data layer yet, do it now
				fillOrders();
			List<BO.OrderForList> listOfOrders = new List<BO.OrderForList>();
			if (Orders.Count > 0)
			{
				foreach (BO.Order order in Orders)										//gets all orders sitting in business layer (with full information)
				{
					BO.OrderForList orderForList = new BO.OrderForList()
					{
						m_id = order.m_id,
						m_customerName = order.m_customerName,
						m_status = order.m_status,
						m_amountOfItems = order.m_items.Count,
						m_totalPrice = order.m_totalPrice
					};
					listOfOrders.Add(orderForList);
				}
			}
			else
			{
				foreach (DO.Order order in dal.Order.ReadAllFiltered())							//since no orders were in the business layer, searches the data layer for orders that may need to be pulled up
				{
					BO.OrderForList orderForList = new BO.OrderForList()
					{
						m_id = order.m_id,
						m_customerName = order.m_customerName
					};

					if (order.m_orderDate != null && order.m_orderDate <= DateTime.Now)
						if (order.m_shipDate != null && order.m_shipDate <= DateTime.Now)
							if (order.m_deliveryDate != null && order.m_deliveryDate <= DateTime.Now)
								orderForList.m_status = BO.Enums.OrderStatus.Delivered;                      //past delivery, shipped and ordered
							else
								orderForList.m_status = BO.Enums.OrderStatus.Shipped;                        //not past delivery, but past order and shipped dates
						else
							orderForList.m_status = BO.Enums.OrderStatus.Ordered;                            //not past shipped, but past ordered
					else
						orderForList.m_status = BO.Enums.OrderStatus.None;                                   //not past shipped, ordered or delivered

					foreach (DO.OrderItem orderItem in dal.OrderItem.ReadAllFiltered())
					{
						if (orderItem.m_orderID == order.m_id)
						{
							orderForList.m_amountOfItems += orderItem.m_amount;
							orderForList.m_totalPrice += (orderItem.m_amount * orderItem.m_price);
						}
					}
					listOfOrders.Add(orderForList);
				}
			}
			return listOfOrders;
		}

		/// <summary>
		/// used when the manager wants to mark an order as being shipped or delivered 
		/// </summary>
		/// <param name="orderId"></param>
		/// <param name="newStatus"></param>
		/// <returns>order that's been updated</returns>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public BO.Order UpdateOrderStatus(int orderId, BO.Enums.OrderStatus newStatus)
		{
			if (Orders.Count == 0)                              //if orders haven't been pulled up from the data layer yet, do it now
				fillOrders();
			BO.Order boOrder = Read(orderId);
			if (boOrder == null)								//if order isn't yet in business layer - get it from the data layer
			{
				try
				{
					DO.Order doOrder = dal.Order.Read(orderId);
					boOrder = new BO.Order();
					boOrder.m_id = doOrder.m_id;
					boOrder.m_customerName = doOrder.m_customerName;
					boOrder.m_customerAddress = doOrder.m_customerAddress;
					boOrder.m_customerEmail = doOrder.m_customerEmail;
					boOrder.m_orderDate = doOrder.m_orderDate;
					boOrder.m_paymentDate = doOrder.m_orderDate;

					boOrder.m_totalPrice = 0;
					foreach (DO.OrderItem doItem in dal.OrderItem.ReadAllFiltered())
					{
						BO.OrderItem boItem = new BO.OrderItem()
						{
							m_id = doItem.m_id,
							m_productID = doItem.m_productID,
							m_amount = doItem.m_amount,
							m_price = doItem.m_price
						};

						boOrder.m_totalPrice += (doItem.m_amount * doItem.m_price);
						boOrder.m_items.Add(boItem);
					}

					boOrder.m_orderDate = doOrder.m_orderDate;
					boOrder.m_deliveryDate = doOrder.m_deliveryDate;
					boOrder.m_shipDate = doOrder.m_shipDate;

					switch (newStatus)
					{
						case BO.Enums.OrderStatus.Ordered:
							if (boOrder.m_orderDate == null || boOrder.m_orderDate > DateTime.Today)           //if it's default or after today set the date to match the new status
							{
								doOrder.m_orderDate = DateTime.Today;
								boOrder.m_orderDate = DateTime.Today;
							}
							boOrder.m_status = BO.Enums.OrderStatus.Ordered;
							break;

						case BO.Enums.OrderStatus.Shipped:
							if (doOrder.m_shipDate == null || doOrder.m_shipDate > DateTime.Today)           //if it's default or after today set the date to match the new status
							{
								doOrder.m_shipDate = DateTime.Today;
								boOrder.m_shipDate = DateTime.Today;
							}
							boOrder.m_status = BO.Enums.OrderStatus.Shipped;
							break;
						case BO.Enums.OrderStatus.Delivered:
							if (doOrder.m_deliveryDate == null || doOrder.m_deliveryDate > DateTime.Today)     //if it's default or after today set the date to match the new status
							{
								doOrder.m_deliveryDate = DateTime.Today;
								boOrder.m_deliveryDate = DateTime.Today;
							}
							boOrder.m_status = BO.Enums.OrderStatus.Delivered;
							break;
						default:
							break;
					}
					dal.Order.Update(doOrder);																			//update order in data layer, in case dates have changed
					boOrder.m_id = Create(boOrder);															//add the id to the order after it's been created successfully in the BL
				}
				catch (DO.idNotFoundException exc)
				{
					Console.WriteLine(orderId);
					throw new BO.dataLayerIdNotFoundException(exc.Message);
				}
				catch (Exception exc)
				{
					Console.WriteLine("Some other problem");
					throw new BO.blGeneralException();
				}
			}
			else																									//if the order already exists in the BL
			{
				int ordIndex = Orders.FindIndex(x => x.m_id == orderId);							//index in the list is needed to make changes to the object itself, not just a copy of it
				if (ordIndex == -1)
				{
					Orders.Add(boOrder);
					ordIndex = Orders.FindIndex(x => x.m_id == orderId);
				}
				switch (newStatus)																	//set status according to the parameter sent
				{
					case BO.Enums.OrderStatus.Ordered:
						Orders[ordIndex].m_status = BO.Enums.OrderStatus.Ordered;
						Orders[ordIndex].m_orderDate = DateTime.Today;
						break;
					case BO.Enums.OrderStatus.Shipped:
						Orders[ordIndex].m_status = BO.Enums.OrderStatus.Shipped;
						Orders[ordIndex].m_shipDate = DateTime.Today;
						break;
					case BO.Enums.OrderStatus.Delivered:
						Orders[ordIndex].m_status = BO.Enums.OrderStatus.Delivered;
						Orders[ordIndex].m_deliveryDate = DateTime.Today;
						break;
					default:
						break;
				}
			}
			Console.WriteLine("Update successful");
			return boOrder;
		}

		/// <summary>
		/// sets up pairs for any relevant order tracking that can be used for the given order
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns>instance of tracker pair</returns>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public BO.OrderTracking OrderTracker(int orderId)
		{
			BO.Order boOrder;
			try
			{
				boOrder = Read(orderId);											//finds order for tracking
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(orderId);
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}

			BO.OrderTracking orderTracking = new BO.OrderTracking();
			orderTracking.m_id = orderId;
			orderTracking.m_status = boOrder.m_status;
			orderTracking.DatePairs = new List<Tuple<DateTime?, string?>>();										//starts an empty list if any pairs that may need to be added

			if (boOrder.m_orderDate != null)
				orderTracking.DatePairs.Add(Tuple.Create(boOrder.m_orderDate, "The order has been ordered"));

			if (boOrder.m_shipDate != null)
				orderTracking.DatePairs.Add(Tuple.Create(boOrder.m_shipDate, "The order has been shipped"));

			if (boOrder.m_deliveryDate != null)
				orderTracking.DatePairs.Add(Tuple.Create(boOrder.m_deliveryDate, "The order has been delivered"));

			Console.WriteLine("Tracking has been set for: " + orderId);
			return orderTracking;
		}

		/// <summary>
		/// validates all entries from users
		/// </summary>
		/// <param name="order"></param>
		/// <exception cref="BO.InputIsInvalidException"></exception>
		private static void InputValidation(BO.Order order)
		{
			//add ' ' (space) to regex expression
			if (order.m_customerName == null || !Regex.IsMatch(order.m_customerName, @"^[a-zA-Z]+$"))
				throw new BO.InputIsInvalidException("Customer Name");

			//add @ and . to regex expression
			if (order.m_customerEmail == null || !Regex.IsMatch(order.m_customerEmail, @"^[a-zA-Z]+\@+[a-zA-Z]+\.+[a-zA-Z]+$"))
				throw new BO.InputIsInvalidException("Customer Email");

			//regex expression (up to 4 digits for number space, street name, space, street type (1st letter caps, up to 3 more lowercase
			if (order.m_customerAddress == null || !Regex.IsMatch(order.m_customerAddress,
				@"^(\d{1,4}) [a-zA-Z\s]+[A-Z]{1}[a-z]{1,3}$"))
				throw new BO.InputIsInvalidException("Customer Address");

			//if orderDate is later than shipDate or delivery date
			if (order.m_shipDate != null)
				if (DateTime.Compare((DateTime)order.m_orderDate, (DateTime)order.m_shipDate) > 0)
					throw new BO.InputIsInvalidException("OrderDate after ship date");
			if (order.m_deliveryDate != null)
				if (DateTime.Compare((DateTime)order.m_orderDate, (DateTime)order.m_deliveryDate) > 0)
				throw new BO.InputIsInvalidException("OrderDate after delivery date");

			//if shipDate is later than delivery date
			if (order.m_shipDate != null || order.m_deliveryDate != null)
				if (DateTime.Compare((DateTime)order.m_shipDate, (DateTime)order.m_deliveryDate) > 0)
					throw new BO.InputIsInvalidException("ShipDate after delivery date");
		}
		
		/// <summary>
		/// initializes a list of BO orders that hold dates and status for consistant referencing
		/// </summary>
		static private void fillOrders()
		{
			foreach (DO.Order doOrder in dal.Order.ReadAllFiltered())
			{
				BO.Order boOrder = new BO.Order();										//grabs data here
				boOrder.m_id = doOrder.m_id;
				boOrder.m_customerName = doOrder.m_customerName;
				boOrder.m_customerAddress = doOrder.m_customerAddress;
				boOrder.m_customerEmail = doOrder.m_customerEmail;
				boOrder.m_orderDate = doOrder.m_orderDate;
				boOrder.m_shipDate = doOrder.m_shipDate;
				boOrder.m_deliveryDate = doOrder.m_deliveryDate;
				boOrder.m_paymentDate = doOrder.m_orderDate;

				if (boOrder.m_orderDate != null && boOrder.m_orderDate <= DateTime.Now)
					if (boOrder.m_shipDate != null && boOrder.m_shipDate <= DateTime.Now)
						if (boOrder.m_deliveryDate != null && boOrder.m_deliveryDate <= DateTime.Now)
							boOrder.m_status = BO.Enums.OrderStatus.Delivered;                      //past delivery, shipped and ordered
						else
							boOrder.m_status = BO.Enums.OrderStatus.Shipped;                        //not past delivery, but past order and shipped dates
					else
						boOrder.m_status = BO.Enums.OrderStatus.Ordered;                            //not past shipped, but past ordered
				else
					boOrder.m_status = BO.Enums.OrderStatus.None;                                   //not past shipped, ordered or delivered

				boOrder.m_totalPrice = 0;
				boOrder.m_items = new();
				foreach (DO.OrderItem doItem in dal.OrderItem.ReadAllFiltered())
				{
					BO.OrderItem boItem = new();													//adds list of orderItems to the order
					if (doItem.m_orderID == doOrder.m_id)
					{
						boItem.m_id = doItem.m_id;
						boItem.m_productID = doItem.m_productID;
						boItem.m_amount = doItem.m_amount;
						boItem.m_price = doItem.m_price;
					}
					else
						continue;

					boOrder.m_totalPrice += (boItem.m_price * boItem.m_amount);
					boOrder.m_items.Add(boItem);
				}

				Orders.Add(boOrder);
			}
		}
	}
}
