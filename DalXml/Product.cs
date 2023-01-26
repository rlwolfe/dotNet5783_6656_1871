using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class Product : IProduct
    {
        public int Create(DO.Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public DO.Product Read(int entityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.Product?> ReadAllFiltered(Func<DO.Product?, bool>? filter = null)
        {
            throw new NotImplementedException();
        }

        public DO.Product ReadWithFilter(Func<DO.Product?, bool>? filter)
        {
            throw new NotImplementedException();
        }

        public void Update(DO.Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
