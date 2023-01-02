using BlApi;
using BO;
using Dal;

namespace BlImplementation
{
	internal class Cart : ICart
	{
		private IDal? dal =  new DalList();

        public BO.Cart AddToCart(BO.Cart cart, int prodID)
        {
            try
            {
                DO.Product product = dal.Product.Read(prodID);
                OrderItem orderItem = cart.m_items.Find(x => x.m_productID == prodID);

                if (orderItem == null)
                {
                    if (product.m_inStock != 0)
                    {
                        orderItem = new OrderItem();
                        orderItem.m_productID = prodID;
                        orderItem.m_price = product.m_price;
                        orderItem.m_amount = 1;
                        cart.m_items.Add(orderItem);
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
			return cart;
		}

        public OrderItem Read(int id)
        {
            OrderItem orderItem = new OrderItem();
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

        public IEnumerable<OrderItem> ReadAll()
        {
            IEnumerable<OrderItem> orderItems = null;
            try
            {
                foreach (DO.OrderItem ordItem in dal.OrderItem.ReadAll())
                {
                    BO.OrderItem orderItem = new OrderItem();
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
                throw new blGeneralException();
            }
            return orderItems;
        }

        public BO.Cart Update(BO.Cart cart, int prodID, int amount)
        {
            if (amount == 0)
            {
                OrderItem toDel = cart.m_items.Find(x => x.m_productID == prodID);

				cart.m_totalPrice -= (toDel.m_price * toDel.m_amount);
                cart.m_items.Remove(toDel);
            }
            return cart;
        }
		public void PlaceOrder(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
        {
            if (customerName == null || customerEmail == null || customerAddress == null)
                throw new BO.InputIsInvalidException("Customer information");
            
            DO.Order doOrder = new DO.Order(customerName, customerEmail, customerAddress, DateTime.Now, DateTime.MinValue, DateTime.MinValue);
            int orderID = dal.Order.Create(doOrder);
            
            foreach (OrderItem orderItem in cart.m_items)
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

                DO.Product tempProd = dal.Product.Read(orderItem.m_productID);
                tempProd.m_inStock -= orderItem.m_amount;
                dal.Product.Update(tempProd);
			}
            BO.Order boOrder = new BO.Order();
        }

	}
}
