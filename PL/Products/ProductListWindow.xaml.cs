using BO;
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
		//static cart - clear when ordered?

		public ProductListWindow(object sender)
		{
			InitializeComponent();
			ProductSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));            //get Enums for combo box list

			if ((sender as Button).Name == "ShowProductsButton")
			{
				ProductsListView.ItemsSource = bl.Product.ManagerListRequest();                 //create listView for manager (productsForList)
				AddProductToCartButton.Visibility = Visibility.Hidden;
			}
			else
			{
				ProductsListView.ItemsSource = bl.Product.CatalogRequest();                     //creates listView for customer (productItems)
				AddProductButton.Visibility = Visibility.Hidden;
			}
		}

		private void AddProductButton_Click(object sender, RoutedEventArgs e)
		{
			bool? window = new AddUpdateProductWindow().ShowDialog();									//open new window to add product
			if (window.HasValue)																//if user finished with add product window
				ProductsListView.ItemsSource = bl?.Product.ManagerListRequest();				//update the product list
		}

		private void ProductSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BO.Enums.Category productCategory = (BO.Enums.Category)ProductSelector.SelectedItem;	//selected category
			if (productCategory == BO.Enums.Category.None)											//view all the products
			{
				ProductsListView.ItemsSource = bl.Product.ManagerListRequest();
				ProductSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
				return;
			}
			if (productCategory is BO.Enums.Category category)										//filtered list
				ProductsListView.ItemsSource = bl?.Product?.ManagerListRequest()?.Select(x => x.m_category == category);

			ProductsListView.ItemsSource = from product in bl?.Product.ManagerListRequest()			//get filtered results
										   where product.m_category == productCategory
										   select product;
		}

		private void ManagerDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (AddProductButton.Visibility == Visibility.Hidden)                               //if currently in customer catalog view
			{
				CustomerDoubleClick(sender, e);
				return;
			}

			BO.ProductForList product = (BO.ProductForList)ProductsListView.SelectedItem;		//take selection from list
			
			if (product == null)																//didn't click an Item on list
				return;
			
			bool? window = new AddUpdateProductWindow(product).ShowDialog();							
			if (window.HasValue)
				ProductsListView.ItemsSource = bl?.Product.ManagerListRequest();                //update product list if user exited out of add product window
		}
		private void CustomerDoubleClick(object sender, MouseButtonEventArgs e)
		{
			BO.ProductItem product = (BO.ProductItem)ProductsListView.SelectedItem;       //take selection from list

			if (product == null)                                                                //didn't click an Item on list
				return;
			
			new ProductDetailsWindow(product).ShowDialog();
		}

		private void AddProductToCartButton_Click(object sender, RoutedEventArgs e)
		{
			if (ProductsListView.SelectedItems.Count != 0)
			{
				foreach (ProductItem product in ProductsListView.SelectedItems)
				{
					//Cart?
					//bl.Cart.AddToCart(cart, product.m_id);
				}
			}
		}

		private void ViewCartButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
