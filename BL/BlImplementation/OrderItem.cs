using BlApi;
using Dal;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation
{
	internal class OrderItem : IOrderItem
	{
		private IDal dal = DalList;
	}
}
