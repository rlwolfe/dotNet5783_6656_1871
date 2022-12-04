using Dal;
using DO;

namespace Dal;

public class DalProduct
{
	public int CreateProduct(Product product)
	{

		if (product.m_id == -1)                                 //ID is null
			product.m_id = DataSource.Config.NextProductNumber;	//add 

		else if (Array.Exists(DataSource.Products, x => x.m_id == product.m_id))
		{

			if (DataSource.Products[Array.IndexOf(DataSource.Products, product.m_id)].m_name == product.m_name)
				throw new Exception("This product already exists");

			else
				throw new Exception("This ID was already taken by a different product");

		}
		DataSource.Products.Append(product);	

		return product.m_id;

	}

	public Product ReadProduct(int ID)
	{			
		if (Array.Exists(DataSource.Products, x => x.m_id == ID))
			return DataSource.Products[Array.IndexOf(DataSource.Products, ID)];

		throw new Exception("A product with that ID was not found!");
	}

	public Product[] ReadAllProducts()
	{
		//maybe need to add new array and return that instead???
		return DataSource.Products;
	}

	public void UpdateProduct(Product product)
	{
		if (Array.Exists(DataSource.Products, x => x.m_id == product.m_id))

			DataSource.Products[Array.IndexOf(DataSource.Products, product)] = product;
		else
			throw new Exception("Product ID doesn't exist");
	}

	public void DeleteProduct(int ID)
	{
		if (Array.Exists(DataSource.Products, x => x.m_id == ID))
			DataSource.Products = DataSource.Products.Where(x => x.m_id != ID).ToArray<Product>();
		
		else
			throw new Exception("A product with that ID was not found!");
	}
}