using BO;
using PL.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Products
{
	/// <summary>
	/// Interaction logic for ProductListWindow.xaml
	/// </summary>
	public partial class ProductListWindow : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();
		static internal Cart cart = new Cart();

		public ProductListWindow(object sender)
		{
			InitializeComponent();
			ProductSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));            //get Enums for combo box list

			if ((sender as Button).Name == "ShowProductsButton")
			{
				ProductsListView.ItemsSource = bl.Product.ManagerListRequest();                 //create listView for manager (productsForList)
				AddProductToCartButton.Visibility = Visibility.Hidden;
				ViewCartButton.Visibility = Visibility.Hidden;
			}
			else
			{
				ProductsListView.ItemsSource = bl.Product.CatalogRequest();                     //creates listView for customer (productItems)
				AddProductButton.Visibility = Visibility.Hidden;
				ViewCartButton.Content = $"View Cart ({cart.Items.Count})";
			}
		}

		private void AddProductButton_Click(object sender, RoutedEventArgs e)
		{
			bool? window = new AddUpdateProductWindow().ShowDialog();                                   //open new window to add product
			if (window.HasValue)                                                                //if user finished with add product window
			{
				if (ProductSelector.SelectedIndex == -1 || (BO.Enums.Category)ProductSelector.SelectedItem == BO.Enums.Category.None)
					ProductsListView.ItemsSource = bl?.Product.ManagerListRequest();                //update product list when user exited out of add product window
				else
					ProductsListView.ItemsSource = from prod in bl?.Product.ManagerListRequest()         //get filtered results
												   where prod.m_category == (BO.Enums.Category)ProductSelector.SelectedItem
												   select prod;
			}
		}

		private void ProductSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BO.Enums.Category productCategory = (BO.Enums.Category)ProductSelector.SelectedItem;    //selected category
			if (productCategory == BO.Enums.Category.None)                                          //view all the products
			{
				if (AddProductButton.Visibility != Visibility.Hidden)                               //if currently in customer catalog view
					ProductsListView.ItemsSource = bl.Product.ManagerListRequest();

				else
					ProductsListView.ItemsSource = bl.Product.CatalogRequest();
			}
			else
			{
				if (AddProductButton.Visibility != Visibility.Hidden)                               //if currently in customer catalog view
					ProductsListView.ItemsSource = from product in bl?.Product.ManagerListRequest()         //get filtered results
												   where product.m_category == productCategory
												   select product;
				else
					ProductsListView.ItemsSource = from product in bl?.Product.CatalogRequest()        //get filtered results
												   where product.m_category == productCategory
												   select product;
			}
		}

		private void ManagerDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (AddProductButton.Visibility == Visibility.Hidden)                               //if currently in customer catalog view
			{
				CustomerDoubleClick(sender, e);
				return;
			}

			BO.ProductForList product = (BO.ProductForList)ProductsListView.SelectedItem;       //take selection from list

			if (product == null)                                                                //didn't click an Item on list
				return;

			bool? window = new AddUpdateProductWindow(product).ShowDialog();
			if (window.HasValue)
			{
				if (ProductSelector.SelectedIndex == -1 || (BO.Enums.Category)ProductSelector.SelectedItem == BO.Enums.Category.None)
					ProductsListView.ItemsSource = bl?.Product.ManagerListRequest();                //update product list when user exited out of update product window
				else
					ProductsListView.ItemsSource = from prod in bl?.Product.ManagerListRequest()         //get filtered results
												   where prod.m_category == (BO.Enums.Category)ProductSelector.SelectedItem
												   select prod;
			}
		}

		private void CustomerDoubleClick(object sender, MouseButtonEventArgs e)
		{
			BO.ProductItem product = (BO.ProductItem)ProductsListView.SelectedItem;       //take selection from list

			if (product == null)                                                                //didn't click an Item on list
				return;

			new ProductDetailsWindow(product).ShowDialog();
			ViewCartButton.Content = $"View Cart ({cart.Items.Count})";
		}

		private void AddProductToCartButton_Click(object sender, RoutedEventArgs e)
		{
			if (ProductsListView.SelectedItem != null)
			{
				ProductItem product = ProductsListView.SelectedItem as ProductItem;
				try
				{
					if (cart.Items.FirstOrDefault(x => x.m_productID == product.m_id) == null)
					{
						if (!bl.Product.CustomerRequest(product.m_id).m_inStock)
							new ErrorWindow("Out of Stock", $"Sorry, {product.m_name} is not in stock").Show();
						else
						{
							bl.Cart.AddToCart(cart, product.m_id);
							new SuccessWindow($"{product.m_name} was successfully added to the cart").Show();
							ViewCartButton.Content = $"View Cart ({cart.Items.Count})";
						}
					}
					else
						new ErrorWindow("Product Duplicate Error", $"{product.m_name} is already in the cart,\nUpdate quantity in 'View Cart'").Show();
				}
				catch (Exception exc)
				{
					throw new plException("Error", "Error Occurred when attempted to add product to cart:\n" + exc.Message);
				}
			}
			else
				new ErrorWindow("Nothing Selected Error", "Please select an item to add to cart").ShowDialog();
		}

		private void ViewCartButton_Click(object sender, RoutedEventArgs e)
		{
			bool? window = new CartWindows.ViewCartWindow().ShowDialog();                                   //open new window to add product
			if (window.HasValue)
				ViewCartButton.Content = $"View Cart ({cart.Items.Count})";
		}
	}
}
