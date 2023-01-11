﻿using BlApi;
using Dal;
using System.Text.RegularExpressions;

namespace BlImplementation
{
	internal class Product : IProduct
	{
		private IDal? dal = new DalList();

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
			catch(DO.idAlreadyExistsException exc) 
			{
				Console.WriteLine(prod.m_id);
				throw new BO.dataLayerIdAlreadyExistsException(exc.Message);
			}
			catch(Exception exc)
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
		/// <returns>list of products</returns>
		/// <exception cref="BO.blGeneralException"></exception>
		public IEnumerable<BO.Product> CatalogRequest()
		{
            List<BO.Product> products = new ();
            try
			{
				foreach (DO.Product prod in dal.Product.ReadAllFiltered())
				{
					BO.Product product = new BO.Product();
					product.m_id = prod.m_id;
					product.m_name = prod.m_name;
					product.m_category = (BO.Enums.Category)(dal.Product.Read(prod.m_id).m_category.GetHashCode());
					product.m_price = prod.m_price;
					product.m_inStock = prod.m_inStock;

					products.Add(product);
				}
			}
			catch(Exception exc)
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
			List<BO.ProductForList> list = new();

			foreach (DO.Product prod in dal.Product.ReadAllFiltered())
			{
				BO.ProductForList productForList = new BO.ProductForList();
				productForList.m_id = prod.m_id;
				productForList.m_name = prod.m_name;
				productForList.m_price = prod.m_price;
				productForList.m_category = (BO.Enums.Category)prod.m_category;

				list.Add(productForList);
			}
			return list;
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
			if (product.m_name == null || !Regex.IsMatch(product.m_name, @"^[a-zA-Z]+$"))
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
