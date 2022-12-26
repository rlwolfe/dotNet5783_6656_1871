using BlApi;
using Dal;
using System.Text.RegularExpressions;

namespace BlImplementation
{
	internal class Product : IProduct
	{
		static IDal? dal = new DalList();

		public int Create(BO.Product product)
		{
			if (product.m_name == null || !Regex.IsMatch(product.m_name, @"^[a-zA-Z]+$"))
				throw new BO.InputIsInvalidException("Name");

			if (product.m_category == null || !Regex.IsMatch(product.m_category, @"^[a-zA-Z]+$"))
				throw new BO.InputIsInvalidException("Category");

			if (product.m_price is <= 0 or > 500)
				throw new BO.InputIsInvalidException("Price");
			
			if (product.m_inStock < 0)
				throw new BO.InputIsInvalidException("Amount in Stock");

			DO.Product prod = new DO.Product(product.m_name,product.m_category, product.m_price, product.m_inStock);
			dal.Product.Create(prod);
			//save id here needs try catch
		}

		public BO.Product Read(int id)
		{
			BO.Product product = new BO.Product();
			product.m_id = dal.Product.Read(id).m_id;
			product.m_name = dal.Product.Read(id).m_name;
			product.m_category = dal.Product.Read(id).m_category;
			product.m_price = dal.Product.Read(id).m_price;
			product.m_inStock = dal.Product.Read(id).m_inStock;

			return product;
		}

		public IEnumerable<BO.Product> ReadAll()
		{
			IEnumerable<BO.Product> products = null;
			foreach (DO.Product prod in dal.Product.ReadAll())
			{
				BO.Product product = new BO.Product();
				product.m_id = prod.m_id;
				product.m_name = prod.m_name;
				product.m_category = prod.m_category;
				product.m_price = prod.m_price;
				product.m_inStock = prod.m_inStock;

				products.Append(product);
			}
			return products;
		}

		public void Update(BO.Product product)
		{

			if (product.m_name == null || !Regex.IsMatch(product.m_name, @"^[a-zA-Z]+$"))
				throw new BO.InputIsInvalidException("Name");

			if (product.m_category == null || !Regex.IsMatch(product.m_category, @"^[a-zA-Z]+$"))
				throw new BO.InputIsInvalidException("Category");

			if (product.m_price is <= 0 or > 500)
				throw new BO.InputIsInvalidException("Price");

			if (product.m_inStock < 0)
				throw new BO.InputIsInvalidException("Amount in Stock");

			DO.Product prod = new DO.Product(product.m_name, product.m_category, product.m_price, product.m_inStock);
			dal.Product.Update(prod);
		}

		public void Delete(int productId)
		{
			dal.Product.Delete(productId);
		}
	}
}
