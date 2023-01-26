using BlApi;
using System.Text.RegularExpressions;
using System.Linq;


namespace BlImplementation
{
    internal class Product : IProduct
	{
		private DalApi.IDal? dal = DalApi.Factory.Get();

		/// <summary>
		/// creates a new product
		/// </summary>
		/// <param name="product"></param>
		/// <returns>id of the created product</returns>
		/// <exception cref="BO.dataLayerIdAlreadyExistsException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public int Create(BO.Product product)
		{
			InputValidation(product);

			DO.Product prod = new DO.Product(product.m_name, (DO.Enums.Category)product.m_category, product.m_price, product.m_inStock);
			try
			{
				dal.Product.Create(prod);
			}
			catch (DO.idAlreadyExistsException exc)
			{
				Console.WriteLine(prod.m_id);
				throw new BO.dataLayerIdAlreadyExistsException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem");
				throw new BO.blGeneralException();
			}
			return prod.m_id;
		}

		/// <summary>
		/// gets product details for customer
		/// </summary>
		/// <param name="id"></param>
		/// <returns>instance of the product for customer</returns>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public BO.ProductItem CustomerRequest(int id)
		{
			DO.Product product;
			try
			{
				product = dal.Product.Read(id);
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(id);
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem");
				throw new BO.blGeneralException();
			}

			BO.ProductItem productItem = new BO.ProductItem()
			{
				m_id = product.m_id,
				m_name = product.m_name,
				m_price = product.m_price,
				m_category = (BO.Enums.Category)product.m_category,
				m_amount = product.m_inStock,
				m_inStock = product.m_inStock > 0
			};

			return productItem;
		}

		/// <summary>
		/// gets all products for customer to view in list
		/// </summary>
		/// <returns>list of productItems</returns>
		/// <exception cref="BO.blGeneralException"></exception>
		public IEnumerable<BO.ProductItem> CatalogRequest()
		{
			List<BO.ProductItem> products = new();
			try
			{
				products.AddRange(from prod in dal.Product.ReadAllFiltered()
								  select new BO.ProductItem()
								  {
									  m_id = prod.Value.m_id,
									  m_name = prod.Value.m_name,
									  m_category = (BO.Enums.Category)(dal.Product.Read(prod.Value.m_id).m_category.GetHashCode()),
									  m_price = prod.Value.m_price,
									  m_amount = prod.Value.m_inStock,
									  m_inStock = prod.Value.m_inStock > 0
								  });
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem");
				throw new BO.blGeneralException();
			}
			return products;
		}

		/// <summary>
		/// gets product details for manager to see
		/// </summary>
		/// <param name="id"></param>
		/// <returns>product</returns>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public BO.Product ManagerRequest(int id)
		{
			BO.Product product = new BO.Product();
			try
			{
				DO.Product doProd = dal.Product.Read(id);
				product.m_id = doProd.m_id;
				product.m_name = doProd.m_name;
				product.m_category = (BO.Enums.Category)(doProd.m_category.GetHashCode());  //converting BO category to DO category
				product.m_price = doProd.m_price;
				product.m_inStock = doProd.m_inStock;
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(id);
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem");
				throw new BO.blGeneralException();
			}
			return product;
		}

		/// <summary>
		/// gets list of all products without amounts for manager to view
		/// </summary>
		/// <returns>list of limited product information</returns>
		public IEnumerable<BO.ProductForList> ManagerListRequest()
		{
			return from prod in dal.Product.ReadAllFiltered()
							  select new BO.ProductForList()
							  {
								  m_id = prod.Value.m_id,
								  m_name = prod.Value.m_name,
								  m_price = prod.Value.m_price,
								  m_category = (BO.Enums.Category)prod.Value.m_category
							  };
		}

		/// <summary>
		/// allows manager to update product information
		/// </summary>
		/// <param name="product"></param>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public void Update(BO.Product product)
		{
			InputValidation(product);

			DO.Product doProd = new DO.Product();
			try
			{
				doProd = dal.Product.Read(product.m_id);
				doProd.m_name = product.m_name;
				doProd.m_category = (DO.Enums.Category)product.m_category;
				doProd.m_price = product.m_price;
				doProd.m_inStock = product.m_inStock;
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(product.m_id);
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem - read product (dl)");
				throw new BO.blGeneralException();
			}

			try
			{
				dal.Product.Update(doProd);
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(doProd.m_id);
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem - update product (dl)");
				throw new BO.blGeneralException();
			}
		}

		/// <summary>
		/// deletes product (if it isn't found in any current orders)
		/// </summary>
		/// <param name="productId"></param>
		/// <exception cref="BO.dataLayerIdNotFoundException"></exception>
		/// <exception cref="BO.blGeneralException"></exception>
		public void Delete(int productId)
		{
			try
			{
				foreach (DO.OrderItem orderItem in dal.OrderItem.ReadAllFiltered())
				{
					if (productId == orderItem.m_productID)
						throw new BO.UnableToExecute("Product was found in orders still", productId);
				}
				dal.Product.Delete(productId);
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(productId);
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (BO.UnableToExecute exc)
			{
				Console.WriteLine("Please remove items from orders and try again\n");
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem");
				throw new BO.blGeneralException();
			}
			Console.WriteLine($"Product with id - {productId} was deleted successfully");
		}

		/// <summary>
		/// validates information from user
		/// </summary>
		/// <param name="product"></param>
		/// <exception cref="BO.InputIsInvalidException"></exception>
		private static void InputValidation(BO.Product product)
		{
			if (product.m_name == null || !Regex.IsMatch(product.m_name, @"^[a-zA-Z\s]+$"))
				throw new BO.InputIsInvalidException("Name");

			if (!Enum.IsDefined(typeof(DO.Enums.Category), product.m_category.GetHashCode()))
				throw new BO.InputIsInvalidException("Category");

			if (product.m_price is <= 0 or > 500)
				throw new BO.InputIsInvalidException("Price");

			if (product.m_inStock < 0)
				throw new BO.InputIsInvalidException("Amount in Stock");
		}
	}
}
