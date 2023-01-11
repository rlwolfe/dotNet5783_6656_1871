using System;
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
		public ProductListWindow()
        {
            InitializeComponent();
			ProductsListView.ItemsSource = bl.Product.ManagerListRequest();						//create listView from products in BL
			ProductSelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));			//get Enums for combo box list
		}

		private void AddProductButton_Click(object sender, RoutedEventArgs e)
		{
			bool? window = new AddUpdateProduct().ShowDialog();									//open new window to add product
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

		private void ProductsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			BO.ProductForList product = (BO.ProductForList)ProductsListView.SelectedItem;		//take selection from list
			bool? window = new AddUpdateProduct(product).ShowDialog();							
			if (window.HasValue)
				ProductsListView.ItemsSource = bl?.Product.ManagerListRequest();                //update product list if user exited out of add product window
		}
	}
}
