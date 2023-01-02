using BlApi;
using Dal;
using System.Linq;
/*
* 	m_id = idCounter++;
m_productID = productID;
m_orderID = orderID;
m_price = Price;
m_amount = amount;
*/
namespace BlImplementation
{
	internal class OrderItem : IOrderItem
	{
		private IDal? dal =  new DalList();

		public int Create(BO.OrderItem orderItem)
		{
            return -1;
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
                    //orderItem.m_orderID = ordItem.m_orderID;
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

        public void Update(BO.OrderItem orderItem)
        {
        }

        public void Delete(int orderItemId)
        {
            try
            {
                dal.OrderItem.Delete(orderItemId);
            }
            catch (DO.idNotFoundException exc)
            {
                Console.WriteLine(orderItemId); //maybe?
                throw new BO.dataLayerIdNotFoundException(exc.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Some other problem"); //maybe?
                throw new BO.blGeneralException();
            }
        }

        public IEnumerable<BO.OrderItem> GetItemsInOrder(int orderID)
		{
            return null;
		}

		public BO.OrderItem GetOrderItemWithProdAndOrderID(int productId, int orderId)
		{
            return null;
		}

		public void SetItemsInOrder()
		{
		}

		public void SetOrderItemWithProdAndOrderID(int productId, int orderId)
		{
		}

    }
}
