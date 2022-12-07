using Dal;
using DO;

namespace Dal;

/// <summary>
/// Class for managing CRUD for Product
/// </summary>
public class DalProduct
{
    ///<summary>
    /// Create function
    /// </summary>
    public int CreateProduct(Product product)
	{

		if (product.m_id == -1)                                 //ID is null
			product.m_id = DataSource.Config.NextProductNumber;	//add 

		else if (Array.Exists(DataSource.Products, x => x.m_id == product.m_id))
				throw new Exception("This product already exists");

		int i = 0;
		while (i < DataSource.Products.Length)
		{
			if (DataSource.Products[i].m_id == 0)
			{
				DataSource.Products[i] = product;
				break;
			}
			else if (i == DataSource.Products.Length - 1)
				Console.WriteLine("The array of products is already full");
			i++;
		}

		return product.m_id;

	}

    ///<summary>
    /// Read function
    /// </summary>
    public Product ReadProduct(int ID)
	{
		if (Array.Find(DataSource.Products, x => x.m_id == ID).m_id == ID)
			return Array.Find(DataSource.Products, x => x.m_id == ID);

		throw new Exception("A product with that ID was not found!");
	}

	public Product[] ReadAllProducts()
	{
		//maybe need to add new array and return that instead???
		return DataSource.Products;
	}

    ///<summary>
    /// Update function
    /// </summary>
    public void UpdateProduct(Product product)
	{
		if (Array.Exists(DataSource.Products, x => x.m_id == product.m_id))
			DataSource.Products[Array.FindIndex(DataSource.Products, x => x.m_id == product.m_id)] = product;

		else
			throw new Exception("Product ID doesn't exist");
	}

    ///<summary>
    /// Delete function
    /// </summary>
    public void DeleteProduct(int ID)
	{
		if (Array.Exists(DataSource.Products, x => x.m_id == ID))
			DataSource.Products = DataSource.Products.Where(x => x.m_id != ID).ToArray<Product>();
		
		else
			throw new Exception("A product with that ID was not found!");
	}
}