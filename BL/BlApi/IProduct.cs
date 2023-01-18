using BO;

namespace BlApi
{
	public interface IProduct
	{
		public int Create(Product product);
		public ProductItem CustomerRequest(int id);
		public IEnumerable<ProductItem?> CatalogRequest();
		public BO.Product ManagerRequest(int id);
		public IEnumerable<BO.ProductForList?> ManagerListRequest();
		public void Update(Product product);
		public void Delete(int productId);
	}
}
