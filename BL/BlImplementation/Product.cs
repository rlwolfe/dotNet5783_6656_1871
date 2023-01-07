﻿using BlApi;
using BO;
using Dal;
using DO;
using System.Text.RegularExpressions;

namespace BlImplementation
{
	internal class Product : IProduct
	{
		private IDal? dal = new DalList();

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
				Console.WriteLine(prod.m_id); //maybe?
				throw new BO.dataLayerIdAlreadyExistsException(exc.Message);
			}
			catch(Exception exc)
			{
                Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
            }
			//save id here needs try catch
			return prod.m_id;			//changed from prod
		}

		public BO.ProductItem CustomerRequest(int id)
		{
			DO.Product product;
			try
			{
				product = dal.Product.Read(id);
			}
            catch (DO.idNotFoundException exc)
            {
                Console.WriteLine(id); //maybe?
                throw new BO.dataLayerIdNotFoundException(exc.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Some other problem"); //maybe?
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

		public IEnumerable<BO.Product> CatalogRequest()
		{
            //IEnumerable<BO.Product> products = null;
            List<BO.Product> products = new ();
            try
			{
				foreach (DO.Product prod in dal.Product.ReadAll())
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
				Console.WriteLine("Some other problem"); //maybe?
                throw new BO.blGeneralException();
            }
			return products;
		}

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
				Console.WriteLine(id); //maybe?
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
			}
			return product;
		}

		public IEnumerable<BO.ProductForList> ManagerListRequest()
		{
			List<BO.ProductForList> list = new();

			foreach (DO.Product prod in dal.Product.ReadAll())
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

		public void Update(BO.Product product)
		{
			InputValidation(product);

            //here be prblem of id
            //DO.Product prod = new DO.Product(product.m_name, (DO.Enums.Category)product.m_category, product.m_price, product.m_inStock);
           
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
                Console.WriteLine(product.m_id); //maybe?
                throw new BO.dataLayerIdNotFoundException(exc.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Some other problem - read product (dl)"); //maybe?
                throw new BO.blGeneralException();
            }
			
            try
			{
				dal.Product.Update(doProd);
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(doProd.m_id); //maybe?
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem - update product (dl)"); //maybe?
				throw new BO.blGeneralException();
			}
		}

		

		public void Delete(int productId)
		{
			try
			{
				foreach (DO.OrderItem orderItem in dal.OrderItem.ReadAll())
				{
					if (productId == orderItem.m_productID)
						throw new BO.UnableToExecute("Product was found in orders still", productId);
				}
				dal.Product.Delete(productId);
			}
			catch (DO.idNotFoundException exc)
			{
				Console.WriteLine(productId); //maybe?
				throw new BO.dataLayerIdNotFoundException(exc.Message);
			}
			catch (BO.UnableToExecute exc)
			{
				Console.WriteLine("Please remove items from orders and try again\n");
			}
			catch (Exception exc)
			{
				Console.WriteLine("Some other problem"); //maybe?
				throw new BO.blGeneralException();
			}
        }
		
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
