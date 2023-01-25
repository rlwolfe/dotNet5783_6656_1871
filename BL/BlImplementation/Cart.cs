using BlApi;
using BO;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace BlImplementation
{
	internal class Cart : ICart
	{
		private IDal? dal = DalApi.Factory.Get();

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
				BO.OrderItem boOrderItem = cart.Items.FirstOrDefault(x => x.m_productID == prodID);         //is the product in the cart?
				if (boOrderItem == null)
				{
					if (product.m_inStock != 0)                                                     //is it in stock?
					{
						boOrderItem = new OrderItem();
						boOrderItem.m_productID = prodID;
						boOrderItem.m_price = product.m_price;
						boOrderItem.m_amount = 1;
						cart.Items?.Add(boOrderItem);
						cart.m_totalPrice = Math.Round(cart.m_totalPrice + boOrderItem.m_price, 2);

					}
					else
					{
						Console.WriteLine("Product out of Stock");
						throw new BO.UnableToExecute("product is out of stock");
					}
				}
				else
				{
					boOrderItem.m_amount += 1;
					cart.m_totalPrice = Math.Round(cart.m_totalPrice + boOrderItem.m_price, 2);
				}
				Console.WriteLine("This is the ID of the product just added: " + prodID);
			}
			catch (DO.idNotFoundException exc)
			{
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (BO.UnableToExecute)
			{

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
			if (amount <= 0)                                                                            //if the user wants to delete the item from the cart
			{
				BO.OrderItem toDel = cart.Items.FirstOrDefault(x => x.m_productID == prodID);

				if (toDel != null)
				{
					cart.m_totalPrice = Math.Round(cart.m_totalPrice - (toDel.m_price * toDel.m_amount), 2);
					cart.Items.Remove(toDel);
				}
			}
			else
			{
				try
				{
					DO.Product product = dal.Product.Read(prodID);                                  //set product if product exists

					BO.OrderItem item = cart.Items.FirstOrDefault(x => x.m_productID == prodID);      //find product in the cart				
					int orderItemIndex = -1;
					if (item != null)
						orderItemIndex = cart.Items.IndexOf(item);

					if (orderItemIndex == -1)
						throw new BO.UnableToExecute("product not yet in cart, please add it to cart first");
					int diff = amount - cart.Items[orderItemIndex].m_amount;

					if (orderItemIndex != -1)                                      //product is in the cart
					{
						if (product.m_inStock >= amount)                    //enough of the product is in stock
						{
							cart.m_totalPrice = Math.Round(cart.m_totalPrice + (product.m_price * diff), 2);      //if new amount < amount in cart it deducts
							cart.Items[orderItemIndex].m_amount = amount;     //if new amount > amount in cart it adds
						}
						else                                                            //if there isn't enough of the product that the customer wanted
						{
							diff = product.m_inStock - cart.Items[orderItemIndex].m_amount;         //find out if there are more in stock than in the cart 
							if (diff >= 0)                                                          //if there is more in stock than in the cart
							{
								cart.Items[orderItemIndex].m_amount += diff;                                //add all that are in stock to the cart
								cart.m_totalPrice = Math.Round(cart.m_totalPrice + (product.m_price * diff), 2);
							}
							else
							{
								cart.Items[orderItemIndex].m_amount = product.m_inStock;                //if there aren't enough in stock, just take all that are in stock
								cart.m_totalPrice = Math.Round(cart.m_totalPrice + (product.m_price * diff), 2);                                        //and deduct what's no longer in the cart from the price
							}
						}

					}
					else
						throw new BO.blGeneralException();

					Console.WriteLine($"The product with the ID {prodID} now has {cart.Items[orderItemIndex].m_amount} in the cart ");
				}
				catch (DO.idNotFoundException exc)
				{
					Console.WriteLine(prodID);
					throw new BO.dataLayerIdNotFoundException(exc.Message);
				}
				catch (BO.UnableToExecute)
				{

				}
				catch (BO.blGeneralException exc)
				{
					Console.WriteLine("Some other problem: " + exc.Message);
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
		/// <exception cref="BO.InputIsInvalidException"></exception>
		/// <exception cref="BO.dataLayerIdAlreadyExistsException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>

		public void PlaceOrder(BO.Cart cart)
		{
			int orderID = -1;
			try
			{
				if (!InputValidation(cart.m_customerName, cart.m_customerEmail, cart.m_customerAddress))                         //as long as the fields are valid
					throw new BO.InputIsInvalidException("Customer information");
				else
				{
					DO.Order doOrder = new DO.Order(cart.m_customerName, cart.m_customerEmail, cart.m_customerAddress, DateTime.Today, null, null);          //create a new order
					orderID = dal.Order.Create(doOrder);                                                                                //set the order ID
				}
			}
			catch (DO.idAlreadyExistsException exc)
			{
				Console.WriteLine(orderID);
				throw new BO.dataLayerIdAlreadyExistsException(exc.Message);
			}
			catch (BO.InputIsInvalidException exc)
			{
				throw new BO.InputIsInvalidException("Customer information");
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem");
				throw new BO.blGeneralException();
			}

			if (cart.Items.Count > 0)
			{
				foreach (BO.OrderItem orderItem in cart.Items)                                                                                  //add all the necessary products to the order
				{
					try
					{
						DO.Product tempProd = dal.Product.Read(orderItem.m_productID);
						if (orderItem.m_amount > tempProd.m_inStock)                  //check that there's enough in stock
							throw new BO.UnableToExecute($"there is not enough {tempProd.m_name} in stock");

						DO.OrderItem newItem = new DO.OrderItem(orderItem.m_productID, orderID, orderItem.m_price, orderItem.m_amount);
						orderItem.m_id = dal.OrderItem.Create(newItem);
						//BO.OrderItem boItem = new BO.OrderItem()

						tempProd.m_inStock -= orderItem.m_amount;
						dal.Product.Update(tempProd);                                                                           //update the product's in stock amount
					}
					catch (DO.idNotFoundException exc)
					{
						Console.WriteLine(orderItem.m_productID);
						throw new BO.dataLayerIdNotFoundException(exc.Message);
					}
					catch (BO.UnableToExecute exc)
					{
						Console.WriteLine(exc.Message);
						cart.Items.First(x => x.m_productID == orderItem.m_productID).m_amount                  //adjust the amount of requested in cart
											= dal.Product.Read(orderItem.m_productID).m_inStock;                //to the amount remaining in store
					}
					catch (Exception exc)
					{
						Console.WriteLine("Some other problem");
						throw new BO.blGeneralException();
					}
				}
			}
		}

		private bool InputValidation(string customerName, string customerEmail, string customerAddress)
		{
			//add ' ' (space) to regex expression
			if (customerName == null || !Regex.IsMatch(customerName, @"^[a-zA-Z\s]+$"))
				throw new BO.InputIsInvalidException("Customer Name");

			//add @ and . to regex expression
			if (customerEmail == null || !Regex.IsMatch(customerEmail, @"^[a-zA-Z]+@+[a-zA-Z]+\.+[a-zA-Z]+$"))
				throw new BO.InputIsInvalidException("Customer Email");

			//regex expression (up to 4 digits for number space, street name, space, street type (1st letter caps, up to 3 more lowercase
			if (customerAddress == null || !Regex.IsMatch(customerAddress, @"^(\d{1,4}) [a-zA-Z\s]+[A-Za-z]{1,3}$"))
				throw new BO.InputIsInvalidException("Customer Address");

			return true;
		}
	}
}
