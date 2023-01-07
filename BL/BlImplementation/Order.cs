using BlApi;
using BO;
using Dal;
using DO;
using System.Text.RegularExpressions;

namespace BlImplementation
{
	internal class Order : IOrder
	{
		static private IDal? dal = new DalList();
		static public List<BO.Order> Orders = new List<BO.Order>();

		static private void fillOrders()
		{
			foreach (DO.Order doOrder in dal.Order.ReadAll())
			{
				BO.Order boOrder = new BO.Order();
				boOrder.m_id = doOrder.m_id;
				boOrder.m_customerName = doOrder.m_customerName;
				boOrder.m_customerAddress = doOrder.m_customerAddress;
				boOrder.m_customerEmail = doOrder.m_customerEmail;
				boOrder.m_orderDate = doOrder.m_orderDate;
				boOrder.m_shipDate = doOrder.m_shipDate;
				boOrder.m_deliveryDate = doOrder.m_deliveryDate;
				boOrder.m_paymentDate = DateTime.MinValue;

				if (boOrder.m_orderDate != DateTime.MinValue && boOrder.m_orderDate <= DateTime.Now)
					if (boOrder.m_shipDate != DateTime.MinValue && boOrder.m_shipDate <= DateTime.Now)
						if (boOrder.m_deliveryDate != DateTime.MinValue && boOrder.m_deliveryDate <= DateTime.Now)
							boOrder.m_status = BO.Enums.OrderStatus.Delivered;                      //past delivery, shipped and ordered
						else
							boOrder.m_status = BO.Enums.OrderStatus.Shipped;                        //not past delivery, but past order and shipped dates
					else
						boOrder.m_status = BO.Enums.OrderStatus.Ordered;                            //not past shipped, but past ordered
				else
					boOrder.m_status = BO.Enums.OrderStatus.None;                                   //not past shipped, ordered or delivered

				boOrder.m_totalPrice = 0;
				boOrder.m_items = new();
				foreach (DO.OrderItem doItem in dal.OrderItem.ReadAll())
				{
					BO.OrderItem boItem = new();
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

		public int Create(BO.Order order)
		{
			if (Orders.Count == 0)
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
				Console.WriteLine(ord.m_id); //maybe?
				throw new BO.dataLayerIdAlreadyExistsException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
			}
			Orders.Add(order);
			//save id here needs try catch
			return ord.m_id;
		}

		public BO.Order Read(int id)
		{
			BO.Order order = new BO.Order();
			try
			{
				if (id > 0)
					order = Orders.Find(x => x.m_id == id);
				else
					throw new BO.InputIsInvalidException("ID");
			}
			catch (BO.InputIsInvalidException)
			{
				Console.WriteLine(id);
			}
			if (order != null)
				return order;

			order = new BO.Order();
			try
			{
				DO.Order doOrd = dal.Order.Read(id);
				order.m_id = doOrd.m_id;
				order.m_customerName = doOrd.m_customerName;
				order.m_customerEmail = doOrd.m_customerEmail;
				order.m_customerAddress = doOrd.m_customerAddress;
				order.m_orderDate = doOrd.m_orderDate;
				order.m_shipDate = doOrd.m_shipDate;
				order.m_deliveryDate = doOrd.m_deliveryDate;
				order.m_paymentDate = DateTime.MinValue;

				if (order.m_orderDate != DateTime.MinValue && order.m_orderDate <= DateTime.Now)
					if (order.m_shipDate != DateTime.MinValue && order.m_shipDate <= DateTime.Now)
						if (order.m_deliveryDate != DateTime.MinValue && order.m_deliveryDate <= DateTime.Now)
							order.m_status = BO.Enums.OrderStatus.Delivered;					  //past delivery, shipped and ordered
						else
							order.m_status = BO.Enums.OrderStatus.Shipped;                        //not past delivery, but past order and shipped dates
					else
						order.m_status = BO.Enums.OrderStatus.Ordered;                            //not past shipped, but past ordered
				else
					order.m_status = BO.Enums.OrderStatus.None;                                   //not past shipped, ordered or delivered



				order.m_totalPrice = 0;
				order.m_items = new();
				foreach (DO.OrderItem doItem in dal.OrderItem.ReadAll())
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

					order.m_totalPrice += (boItem.m_price * boItem.m_amount);
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
				Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
			}
			return order;
		}

		public IEnumerable<BO.OrderForList> ReadAll()
		{
			if (Orders.Count == 0)
				fillOrders();
			List<BO.OrderForList> listOfOrders = new List<BO.OrderForList>();
			if (Orders.Count > 0)
			{
				foreach (BO.Order order in Orders)
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
				foreach (DO.Order order in dal.Order.ReadAll())
				{
					BO.OrderForList orderForList = new BO.OrderForList()
					{
						m_id = order.m_id,
						m_customerName = order.m_customerName
					};

					if (order.m_orderDate != DateTime.MinValue && order.m_orderDate <= DateTime.Now)
						if (order.m_shipDate != DateTime.MinValue && order.m_shipDate <= DateTime.Now)
							if (order.m_deliveryDate != DateTime.MinValue && order.m_deliveryDate <= DateTime.Now)
								orderForList.m_status = BO.Enums.OrderStatus.Delivered;                      //past delivery, shipped and ordered
							else
								orderForList.m_status = BO.Enums.OrderStatus.Shipped;                        //not past delivery, but past order and shipped dates
						else
							orderForList.m_status = BO.Enums.OrderStatus.Ordered;                            //not past shipped, but past ordered
					else
						orderForList.m_status = BO.Enums.OrderStatus.None;                                   //not past shipped, ordered or delivered

					foreach (DO.OrderItem orderItem in dal.OrderItem.ReadAll())
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

		public BO.Order UpdateOrderStatus(int orderId, BO.Enums.OrderStatus newStatus)
		{
			if (Orders.Count == 0)
				fillOrders();
			BO.Order boOrder = Read(orderId);
			if (boOrder == null)
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
					boOrder.m_paymentDate = DateTime.MinValue; //how to determine?

					boOrder.m_totalPrice = 0;
					foreach (DO.OrderItem doItem in dal.OrderItem.ReadAll())
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
							if (boOrder.m_orderDate == DateTime.MinValue || boOrder.m_orderDate > DateTime.Today)           //if it's default or after today
							{
								doOrder.m_orderDate = DateTime.Today;
								boOrder.m_orderDate = DateTime.Today;
								boOrder.m_status = BO.Enums.OrderStatus.Ordered;
							}
							else
								boOrder.m_status = BO.Enums.OrderStatus.Ordered;
							break;

						case BO.Enums.OrderStatus.Shipped:
							if (doOrder.m_shipDate == DateTime.MinValue || doOrder.m_shipDate > DateTime.Today)
							{
								doOrder.m_shipDate = DateTime.Today;
								boOrder.m_shipDate = DateTime.Today;
								boOrder.m_status = BO.Enums.OrderStatus.Shipped;
							}
							else
								boOrder.m_status = BO.Enums.OrderStatus.Shipped;
							break;
						case BO.Enums.OrderStatus.Delivered:
							if (doOrder.m_deliveryDate == DateTime.MinValue || doOrder.m_deliveryDate > DateTime.Today)
							{
								doOrder.m_deliveryDate = DateTime.Today;
								boOrder.m_deliveryDate = DateTime.Today;
								boOrder.m_status = BO.Enums.OrderStatus.Delivered;
							}
							else
								boOrder.m_status = BO.Enums.OrderStatus.Delivered;
							break;
						default:
							break;
					}
					dal.Order.Update(doOrder);
					boOrder.m_id = Create(boOrder);
				}
				catch (DO.idNotFoundException exc)
				{
					Console.WriteLine(orderId); //maybe?
					throw new BO.dataLayerIdNotFoundException(exc.Message);
				}
				catch (Exception exc)
				{
					Console.WriteLine("Some other problem"); //maybe?
					throw new BO.blGeneralException();
				}
			}
			else
			{
				int ordIndex = Orders.FindIndex(x => x.m_id == orderId);
				if (ordIndex == -1)
				{
					Orders.Add(boOrder);
					ordIndex = Orders.FindIndex(x => x.m_id == orderId);
				}
				switch (newStatus)
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

		public BO.OrderTracking OrderTracker(int orderId)
		{
			BO.Order boOrder;
			try
			{
				DO.Order doOrder = dal.Order.Read(orderId);
				boOrder = Read(orderId);

			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(orderId); //maybe?
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
			}
			BO.OrderTracking orderTracking = new BO.OrderTracking();
			orderTracking.m_id = orderId;
			orderTracking.m_status = boOrder.m_status;          //date check?
			orderTracking.DatePairs = new List<Tuple<DateTime, string>>();

			if (boOrder.m_orderDate != DateTime.MinValue)
				orderTracking.DatePairs.Add(Tuple.Create(boOrder.m_orderDate, "The order has been ordered"));

			if (boOrder.m_shipDate != DateTime.MinValue)
				orderTracking.DatePairs.Add(Tuple.Create(boOrder.m_shipDate, "The order has been shipped"));

			if (boOrder.m_deliveryDate != DateTime.MinValue)
				orderTracking.DatePairs.Add(Tuple.Create(boOrder.m_deliveryDate, "The order has been delivered"));

			return orderTracking;
		}

		private static void InputValidation(BO.Order order)
		{
			//add ' ' (space) to regex expression
			if (order.m_customerName == null || !Regex.IsMatch(order.m_customerName, @"^[a-zA-Z]+$"))
				throw new BO.InputIsInvalidException("Customer Name");
			//add @ and . to regex expression
			if (order.m_customerEmail == null || !Regex.IsMatch(order.m_customerEmail, @"^[^@\s] +@[^@\s] +\.[^@\s]+$"))
				throw new BO.InputIsInvalidException("Customer Email");
			//regex expression (up to 4 digits for number space, street name, space, street type (1st letter caps, up to 3 more lowercase
			if (order.m_customerAddress == null || !Regex.IsMatch(order.m_customerAddress,
				@"^(\d{1,4}) [a-zA-Z\s]+[A-Z]{1}[a-z]{1,3}$"))
				throw new BO.InputIsInvalidException("Customer Address");
			//if orderDate is later or equal to shipDate or delivery date
			if ((DateTime.Compare(order.m_orderDate, order.m_shipDate) > 0 || DateTime.Compare(order.m_orderDate, order.m_shipDate) == 0) ||
				(DateTime.Compare(order.m_orderDate, order.m_deliveryDate) > 0 || DateTime.Compare(order.m_orderDate, order.m_shipDate) == 0))
				throw new BO.InputIsInvalidException("OrderDate");
			//if shipDate is later than delivery date
			if (DateTime.Compare(order.m_shipDate, order.m_deliveryDate) > 0)
				throw new BO.InputIsInvalidException("ShipDate");
		}
	}
}
