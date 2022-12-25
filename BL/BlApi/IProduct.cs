using BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
	public interface IProduct
	{
		public void Create(Product product);
		public Product Read(int id);
		public IEnumerable<Product> ReadAll();
		public void Update(Product product);
		public void Delete(int productId);
	}
}
