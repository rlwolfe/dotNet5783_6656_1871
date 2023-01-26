using System;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    sealed internal class DalXml : IDal
    {
        
        public static IDal Instance { get; } = new DalXml();
        private DalXml() { }
       
    
        public IOrder Order { get; } = new Dal.Order();

        public IProduct Product { get; } = new Dal.Product();

        public IOrderItem OrderItem { get; } = new Dal.OrderItem();
    }
}
