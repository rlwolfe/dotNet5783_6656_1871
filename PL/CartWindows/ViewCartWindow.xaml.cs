using BlApi;
using BO;
using PL.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.CartWindows
{
	/// <summary>
	/// Interaction logic for ViewCartWindow.xaml
	/// </summary>
	public partial class ViewCartWindow : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();
		public ViewCartWindow()
		{
			InitializeComponent();
			ItemsListBox.ItemsSource = ProductListWindow.cart.Items;
			TotalPriceTextBox.Text = ProductListWindow.cart.m_totalPrice.ToString();
		}

		private void IncreaseButton_Click(object sender, RoutedEventArgs e)
		{
			if (ItemsListBox.SelectedItem != null)
			{
				OrderItem order = (OrderItem)ItemsListBox.SelectedItem;
				bl.Cart.Update(ProductListWindow.cart, order.m_productID, order.m_amount + 1);


				ItemsListBox.Items.Refresh();
				TotalPriceTextBox.Text = ProductListWindow.cart.m_totalPrice.ToString();
			}
			else
				new ErrorWindow("Nothing Selected", "Please select a product to increase").Show();
		}

		private void DecreaseButton_Click(object sender, RoutedEventArgs e)
		{
			if (ItemsListBox.SelectedItem != null)								//first of selected products is decreased
			{
				OrderItem order = (OrderItem)ItemsListBox.SelectedItem;
				bl.Cart.Update(ProductListWindow.cart, order.m_productID, order.m_amount - 1);

				ItemsListBox.Items.Refresh();
				TotalPriceTextBox.Text = ProductListWindow.cart.m_totalPrice.ToString();
			}
			else
				new ErrorWindow("Nothing Selected", "Please select a product to decrease").Show();
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (ItemsListBox.SelectedItem != null)                              //first of selected products is removed from cart
			{
				OrderItem order = (OrderItem)ItemsListBox.SelectedItem;
				bl.Cart.Update(ProductListWindow.cart, order.m_productID, 0);

				ItemsListBox.Items.Refresh();
				TotalPriceTextBox.Text = ProductListWindow.cart.m_totalPrice.ToString();
			}
			else
				new ErrorWindow("Nothing Selected", "Please select a product to remove").Show();
		}

		private void ClearCartButton_Click(object sender, RoutedEventArgs e)
		{
			while (ProductListWindow.cart.Items.Count != 0)
			{
				bl.Cart.Update(ProductListWindow.cart, ProductListWindow.cart.Items.First().m_productID, 0);
			}

			ItemsListBox.Items.Refresh();
			TotalPriceTextBox.Text = ProductListWindow.cart.m_totalPrice.ToString();
		}

		private void PlaceOrderButton_Click(object sender, RoutedEventArgs e)
		{
			new Orders.OrderDetailsWindow().ShowDialog();
			this.Close();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
