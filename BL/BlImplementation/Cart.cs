using BlApi;

namespace BlImplementation
{
	internal class Cart : ICart
	{
		private IDal? dal = new Dal.DalList();

		public BO.Cart AddToCart(BO.Cart cart, int prodID)
		{
			try
			{
				DO.Product product = dal.Product.Read(prodID);
				//this line is the issue - jumps straight to gen exception - null reference -
				//? stops it from immediately exiting from the following line
				BO.OrderItem orderItem = cart.m_items?.Find(x => x.m_productID == prodID);
				if (orderItem == null)
                {
					if (product.m_inStock != 0)
					{
						orderItem = new BO.OrderItem();
						orderItem.m_productID = prodID;
						orderItem.m_price = product.m_price;
						orderItem.m_amount = 1;
						cart.m_items?.Add(orderItem);
						cart.m_totalPrice += orderItem.m_price;
					}
					else
					{
						Console.WriteLine("Product out of Stock");
						throw new BO.blGeneralException();
					}
				}
				else
				{
					orderItem.m_amount += 1;
					cart.m_totalPrice += orderItem.m_price;
				}
			}
			catch (DO.idNotFoundException exc)
			{
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch(Exception exc)
			{
                Console.WriteLine("Some other problem"); //maybe?
                throw new BO.blGeneralException();
            }
        
			return cart;
		}

		public BO.OrderItem Read(int id)
		{
			BO.OrderItem orderItem = new BO.OrderItem();
			try
			{
				orderItem.m_id = dal.OrderItem.Read(id).m_id;
				orderItem.m_productID = dal.OrderItem.Read(id).m_productID;
				orderItem.m_price = dal.OrderItem.Read(id).m_price;
				orderItem.m_amount = dal.OrderItem.Read(id).m_amount;
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(id); //maybe?
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
			}
			return orderItem;

		}

		public IEnumerable<BO.OrderItem> ReadAll()
		{
			IEnumerable<BO.OrderItem> orderItems = null;
			try
			{
				foreach (DO.OrderItem ordItem in dal.OrderItem.ReadAll())
				{
					BO.OrderItem orderItem = new BO.OrderItem();
					orderItem.m_id = ordItem.m_id;
					orderItem.m_productID = ordItem.m_productID;
					orderItem.m_price = ordItem.m_price;
					orderItem.m_amount = ordItem.m_amount;

					orderItems.Append(orderItem);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
			}
			return orderItems;
		}

		public BO.Cart Update(BO.Cart cart, int prodID, int amount)
		{
			if (amount == 0)
			{
				BO.OrderItem toDel = cart.m_items.Find(x => x.m_productID == prodID);

				cart.m_totalPrice -= (toDel.m_price * toDel.m_amount);
				cart.m_items.Remove(toDel);
			}
			else
			{
				try
				{
					DO.Product product = dal.Product.Read(prodID);
					int orderItemIndex = cart.m_items.FindIndex(x => x.m_productID == prodID);
					int diff = amount - cart.m_items[orderItemIndex].m_amount;

					if (orderItemIndex != -1)                                      //product is in the cart
					{
						if (product.m_inStock >= amount)                    //enough of the product is in stock
						{
							cart.m_totalPrice += (product.m_price * diff);      //if new amount < amount in cart it deducts
							cart.m_items[orderItemIndex].m_amount = amount;     //if new amount > amount in cart it adds
						}
						else
						{
							diff = product.m_inStock - cart.m_items[orderItemIndex].m_amount;
							if (diff >= 0)
							{
								cart.m_items[orderItemIndex].m_amount += diff;
								cart.m_totalPrice += (product.m_price * diff);
							}
							else
							{
								cart.m_items[orderItemIndex].m_amount = product.m_inStock;
								cart.m_totalPrice += (product.m_price * diff);
							}
						}

					}
					else
						throw new BO.blGeneralException();
				}
                catch (DO.idNotFoundException exc)
                {
                    Console.WriteLine(prodID); //maybe?
                    throw new BO.dataLayerIdNotFoundException(exc.Message);
                }
                catch (BO.blGeneralException exc)
                {
                    Console.WriteLine("Some other problem: " +exc.Message); //maybe?
                    throw new BO.blGeneralException();
                }
                catch (Exception exc)
				{
					Console.WriteLine("Some other problem"); //maybe?
					throw new BO.blGeneralException();
				}
			}
			return cart;
		}

		public void PlaceOrder(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
		{
			if (customerName == null || customerEmail == null || customerAddress == null)
				throw new BO.InputIsInvalidException("Customer information");

			int orderID = -1;
			try
			{
				DO.Order doOrder = new DO.Order(customerName, customerEmail, customerAddress, DateTime.Today, DateTime.MinValue, DateTime.MinValue);
				orderID = dal.Order.Create(doOrder);
			}
			catch (DO.idAlreadyExistsException exc)
			{
				Console.WriteLine(orderID); //maybe?
				throw new BO.dataLayerIdAlreadyExistsException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
			}

			foreach (BO.OrderItem orderItem in cart.m_items)
			{
				if (orderItem.m_amount <= 0 || orderItem.m_amount > dal.Product.ReadAll().First().m_inStock)
					throw new BO.blGeneralException();

				DO.OrderItem newItem = new DO.OrderItem();
				newItem.m_id = orderItem.m_id;
				newItem.m_productID = orderItem.m_productID;
				newItem.m_price = orderItem.m_price;
				newItem.m_amount = orderItem.m_amount;
				newItem.m_orderID = orderID;

				dal.OrderItem.Create(newItem);

				try
				{
					DO.Product tempProd = dal.Product.Read(orderItem.m_productID);
					tempProd.m_inStock -= orderItem.m_amount;
					dal.Product.Update(tempProd);
				}
				catch (DO.idNotFoundException exc)
				{
					Console.WriteLine(orderItem.m_productID); //maybe?
					throw new BO.dataLayerIdNotFoundException(exc.Message);
				}
				catch (Exception exc)
				{
					Console.WriteLine("Some other problem"); //maybe?
					throw new BO.blGeneralException();
				}
			}
			BO.Order boOrder = new BO.Order()
			{
				m_customerName = customerName,
				m_customerEmail = customerEmail,
				m_customerAddress = customerAddress,
				m_orderDate = DateTime.Today,
				m_paymentDate = DateTime.MinValue,
				m_shipDate = DateTime.MinValue,
				m_deliveryDate = DateTime.MinValue,
				m_status = BO.Enums.OrderStatus.Ordered,
				m_totalPrice = cart.m_totalPrice,
				m_items = cart.m_items
			};
			try
			{
				Order blOrder = new Order();
				blOrder.Create(boOrder);
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(boOrder.m_id); //maybe?
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
		}

	}
}
