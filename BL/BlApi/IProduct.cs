using BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
	public interface IProduct
	{
		public int Create(Product product);
		public ProductItem CustomerRequest(int id);
		public IEnumerable<Product?> CatalogRequest();
		public BO.Product ManagerRequest(int id);
		public IEnumerable<BO.ProductForList?> ManagerListRequest();
		public void Update(Product product);
		public void Delete(int productId);
	}
}
