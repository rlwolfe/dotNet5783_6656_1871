using BlApi;
using DO;

namespace Dal;

/// <summary>
/// Class for managing CRUD for Product
/// </summary>
internal class DalProduct : IProduct
{
    ///<summary>
    /// Create function
    /// </summary>
    public int Create(Product product)
	{
		if (DataSource.Products.Exists(x => x?.m_id == product.m_id))
			throw new idAlreadyExistsException("Product");

		else if (DataSource.Products.Count == DataSource.Products.Capacity)
			Console.WriteLine("The products list is already full");
		
		else
			DataSource.Products.Add(product);

		return product.m_id;
	}

    ///<summary>
    /// Read function
    /// </summary>
    public Product Read(int ID)
	{
		if (DataSource.Products.Exists(x => x?.m_id == ID))
			return (Product)DataSource.Products.Find(x => x?.m_id == ID);

		throw new idNotFoundException("Product");
	}
	public Product ReadWithFilter(Func<Product?, bool>? filter)
	{
		if (filter == null)
		{
			throw new ArgumentNullException(nameof(filter));//filter is null
		}
		foreach (Product? product in DataSource.Products)
		{
			if (product != null && filter(product))
			{
				return (Product)product;
			}
		}
		throw new DO.EntityNotFoundException();
	}
	public IEnumerable<Product?> ReadAllFiltered(Func<Product?, bool>? filter)
	{
		if (filter == null)                                 //return all
			return DataSource.Products;

		return from product in DataSource.Products      //return filtered list
			   where filter(product) select product;
	}

    ///<summary>
    /// Update function
    /// </summary>
    public void Update(Product product)
	{
		if (DataSource.Products.Exists(x => x?.m_id == product.m_id))
		{
			int temp = DataSource.Products.FindIndex(x => x?.m_id == product.m_id);
			DataSource.Products.RemoveAt(temp);
			DataSource.Products.Insert(temp, product);
		}
		else
			throw new idNotFoundException("Product");
	}

    ///<summary>
    /// Delete function
    /// </summary>
    public void Delete(int ID)
	{
		if (DataSource.Products.Exists(x => x?.m_id == ID))
			DataSource.Products.RemoveAt(DataSource.Products.FindIndex(x => x?.m_id == ID));		
		else
			throw new idNotFoundException("Product");
	}
}