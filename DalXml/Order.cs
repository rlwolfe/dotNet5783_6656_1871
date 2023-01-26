using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class Order : IOrder
    {
        public int Create(DO.Order entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public DO.Order Read(int entityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.Order?> ReadAllFiltered(Func<DO.Order?, bool>? filter = null)
        {
            throw new NotImplementedException();
        }

        public DO.Order ReadWithFilter(Func<DO.Order?, bool>? filter)
        {
            throw new NotImplementedException();
        }

        public void Update(DO.Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
