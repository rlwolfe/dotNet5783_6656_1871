using BlApi;
using Dal;
using System.Text.RegularExpressions;

namespace BlImplementation
{
    internal class Order : IOrder
    {
        private IDal? dal = new DalList();

        public int Create(BO.Order order)
		{
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
			//save id here needs try catch
			return -1;
		}

		public BO.Order Read(int id)
        {
            BO.Order order = new BO.Order();
            try
            {
                order.m_id = dal.Order.Read(id).m_id;
                order.m_customerName = dal.Order.Read(id).m_customerName;
                order.m_customerEmail = dal.Order.Read(id).m_customerEmail;
                order.m_customerAddress = dal.Order.Read(id).m_customerAddress;
                order.m_orderDate = dal.Order.Read(id).m_orderDate;
                order.m_shipDate = dal.Order.Read(id).m_shipDate;
                order.m_deliveryDate = dal.Order.Read(id).m_deliveryDate;
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
            return order;
        }

        public IEnumerable<BO.Order> ReadAll()
        {
            IEnumerable<BO.Order> orders = null;
            try
            {
                foreach (DO.Order ord in dal.Order.ReadAll())
                {
                    BO.Order order = new BO.Order();
                    order.m_id = ord.m_id;
                    order.m_customerName = ord.m_customerName;
                    order.m_customerEmail = ord.m_customerEmail;
                    order.m_customerAddress = ord.m_customerAddress;
                    order.m_orderDate = ord.m_orderDate;
                    order.m_shipDate = ord.m_shipDate;
                    order.m_deliveryDate = ord.m_deliveryDate;

                    orders.Append(order);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Some other problem"); //maybe?
                throw new BO.blGeneralException();
            }
            return orders;
        }

        public void Update(BO.Order order)
        {
            InputValidation(order);

            DO.Order ord = new DO.Order(order.m_customerName, order.m_customerEmail, order.m_customerAddress, order.m_orderDate,
               order.m_shipDate, order.m_deliveryDate);
            try
            {
                dal.Order.Update(ord);
            }
            catch (DO.idNotFoundException exc)
            {
                Console.WriteLine(order.m_id); //maybe?
                throw new BO.dataLayerIdNotFoundException(exc.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Some other problem"); //maybe?
                throw new BO.blGeneralException();
            }
        }

        public void Delete(int orderId)
        {
            try
            {
                dal.Order.Delete(orderId);
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
