using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class OrderItem : IOrderItem
    {
        public int Create(DO.OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.OrderItem?> GetItemsInOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public DO.OrderItem GetOrderItemWithProdAndOrderID(int productId, int orderId)
        {
            throw new NotImplementedException();
        }

        public DO.OrderItem Read(int entityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.OrderItem?> ReadAllFiltered(Func<DO.OrderItem?, bool>? filter = null)
        {
            throw new NotImplementedException();
        }

        public DO.OrderItem ReadWithFilter(Func<DO.OrderItem?, bool>? filter)
        {
            throw new NotImplementedException();
        }

        public void SetItemsInOrder(int orderID)
        {
            throw new NotImplementedException();
        }

        public void SetOrderItemWithProdAndOrderID(int productId, int orderId)
        {
            throw new NotImplementedException();
        }

        public void Update(DO.OrderItem entity)
        {
            throw new NotImplementedException();
        }
    }


}
