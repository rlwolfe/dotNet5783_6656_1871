using BlApi;

namespace BlImplementation
{
	internal class Cart : ICart
	{
		private IDal? dal = new Dal.DalList();

		/// <summary>
		/// adds orderItem to cart
		/// </summary>
		/// <param name="cart"></param>
		/// <param name="prodID"></param>
		/// <returns>instance of the cart</returns>
		/// <exception cref="BO.blGeneralException"></exception>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		public BO.Cart AddToCart(BO.Cart cart, int prodID)
		{
			try
			{
				DO.Product product = dal.Product.Read(prodID);
				BO.OrderItem boOrderItem = cart.m_items?.Find(x => x.m_productID == prodID);
				if (boOrderItem == null)
                {
					if (product.m_inStock != 0)
					{
						boOrderItem = new BO.OrderItem();
						boOrderItem.m_productID = prodID;
						boOrderItem.m_price = product.m_price;
						boOrderItem.m_amount = 1;
						cart.m_items?.Add(boOrderItem);
						cart.m_totalPrice += boOrderItem.m_price;
					}
					else
					{
						Console.WriteLine("Product out of Stock");
						throw new BO.blGeneralException();
					}
				}
				else
				{
					boOrderItem.m_amount += 1;
					cart.m_totalPrice += boOrderItem.m_price;
				}
			}
			catch (DO.idNotFoundException exc)
			{
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch(Exception exc)
			{
                Console.WriteLine("Some other problem"); 
                throw new BO.blGeneralException();
            }
        
			return cart;
		}

		/// <summary>
		/// gets the requested orderItem from the data layer and returns and instance of it in the BL
		/// </summary>
		/// <param name="id"></param>
		/// <returns>orderItem requested</returns>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
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
				Console.WriteLine(id); 
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); 
				throw new BO.blGeneralException();
			}
			return orderItem;

		}

		/// <summary>
		/// updates the amount of an item in the cart
		/// </summary>
		/// <param name="cart"></param>
		/// <param name="prodID"></param>
		/// <param name="amount"></param>
		/// <returns>instance of the cart</returns>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public BO.Cart Update(BO.Cart cart, int prodID, int amount)
		{
			if (amount == 0)																			//if the user wants to delete the item from the cart
			{
				BO.OrderItem toDel = cart.m_items.Find(x => x.m_productID == prodID);

				if (toDel != null)
				{
					cart.m_totalPrice -= (toDel.m_price * toDel.m_amount);
					cart.m_items.Remove(toDel);
				}
			}
			else
			{
				try
				{
					DO.Product product = dal.Product.Read(prodID);									//find product in the cart
					int orderItemIndex = cart.m_items.FindIndex(x => x.m_productID == prodID);
					int diff = amount - cart.m_items[orderItemIndex].m_amount;

					if (orderItemIndex != -1)                                      //product is in the cart
					{
						if (product.m_inStock >= amount)                    //enough of the product is in stock
						{
							cart.m_totalPrice += (product.m_price * diff);      //if new amount < amount in cart it deducts
							cart.m_items[orderItemIndex].m_amount = amount;     //if new amount > amount in cart it adds
						}
						else															//if there isn't enough of the product that the customer wanted
						{
							diff = product.m_inStock - cart.m_items[orderItemIndex].m_amount;			//find out if there are more in stock than in the cart 
							if (diff >= 0)															//if there is more in stock than in the cart
							{
								cart.m_items[orderItemIndex].m_amount += diff;								//add all that are in stock to the cart
								cart.m_totalPrice += (product.m_price * diff);
							}
							else
							{
								cart.m_items[orderItemIndex].m_amount = product.m_inStock;				//if there aren't enough in stock, just take all that are in stock
								cart.m_totalPrice += (product.m_price * diff);										//and deduct what's no longer in the cart from the price
							}
						}

					}
					else
						throw new BO.blGeneralException();
				}
                catch (DO.idNotFoundException exc)
                {
                    Console.WriteLine(prodID); 
                    throw new BO.dataLayerIdNotFoundException(exc.Message);
                }
                catch (BO.blGeneralException exc)
                {
                    Console.WriteLine("Some other problem: " +exc.Message); 
                    throw new BO.blGeneralException();
                }
                catch (Exception exc)
				{
					Console.WriteLine("Some other problem"); 
					throw new BO.blGeneralException();
				}
			}
			return cart;
		}

		/// <summary>
		/// lets customer place the actual order from what's in the cart
		/// </summary>
		/// <param name="cart"></param>
		/// <param name="customerName"></param>
		/// <param name="customerEmail"></param>
		/// <param name="customerAddress"></param>
		/// <exception cref="BO.InputIsInvalidException"></exception>
		/// <exception cref="BO.dataLayerIdAlreadyExistsException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		public void PlaceOrder(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
		{
			if (customerName == null || customerEmail == null || customerAddress == null)							//as long as the fields aren't empty (the test layer is checking the rest of the logic
				throw new BO.InputIsInvalidException("Customer information");

			int orderID = -1;
			try
			{
				DO.Order doOrder = new DO.Order(customerName, customerEmail, customerAddress, DateTime.Today, DateTime.MinValue, DateTime.MinValue);			//create a new order
				orderID = dal.Order.Create(doOrder);																											//set the order ID
			}
			catch (DO.idAlreadyExistsException exc)
			{
				Console.WriteLine(orderID); 
				throw new BO.dataLayerIdAlreadyExistsException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); 
				throw new BO.blGeneralException();
			}

			foreach (BO.OrderItem orderItem in cart.m_items)																					//add all the necessary products to the order
			{
				if (orderItem.m_amount <= 0 || orderItem.m_amount > dal.Product.Read(orderItem.m_productID).m_inStock)					//check that there's enough in stock
					throw new BO.blGeneralException();

				DO.OrderItem newItem = new DO.OrderItem(orderItem.m_productID, orderID, orderItem.m_price, orderItem.m_amount);
				orderItem.m_id = dal.OrderItem.Create(newItem);

				try
				{
					DO.Product tempProd = dal.Product.Read(orderItem.m_productID);
					tempProd.m_inStock -= orderItem.m_amount;
					dal.Product.Update(tempProd);																			//update the product's in stock amount
				}
				catch (DO.idNotFoundException exc)
				{
					Console.WriteLine(orderItem.m_productID); 
					throw new BO.dataLayerIdNotFoundException(exc.Message);
				}
				catch (Exception exc)
				{
					Console.WriteLine("Some other problem"); 
					throw new BO.blGeneralException();
				}
			}
			BO.Order boOrder = new BO.Order()													//create final BL order for proccessing 
			{
				m_customerName = customerName,
				m_customerEmail = customerEmail,
				m_customerAddress = customerAddress,
				m_orderDate = DateTime.Today,
				m_paymentDate = DateTime.Today,
				m_shipDate = DateTime.MinValue,
				m_deliveryDate = DateTime.MinValue,
				m_status = BO.Enums.OrderStatus.Ordered,
				m_totalPrice = cart.m_totalPrice,
				m_items = cart.m_items
			};
			try
			{
				Order blOrder = new Order();
				blOrder.Create(boOrder);								//creation happens here
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(boOrder.m_id); 
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
		}

	}
}
