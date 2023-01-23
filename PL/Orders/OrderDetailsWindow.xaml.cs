using BlApi;
using BO;
using PL.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Orders
{
	/// <summary>
	/// Interaction logic for OrderDetailsWindow.xaml
	/// </summary>
	public partial class OrderDetailsWindow : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();

		public OrderDetailsWindow()
		{
			InitializeComponent();
			StatusBox.Visibility = Visibility.Hidden;
			OrderDatePicker.Visibility = Visibility.Hidden;
			PaymentDatePicker.Visibility = Visibility.Hidden;                   //hide date fields (for manager update)
			ShipDatePicker.Visibility = Visibility.Hidden;
			DelivDatePicker.Visibility = Visibility.Hidden;
			IDBox.Visibility = Visibility.Hidden;                               //no need to see empty ID field (assigned after creation)

			PlaceOrderButton.Visibility = Visibility.Visible;
			UpdateButton.Visibility = Visibility.Hidden;                        //correct button options (can't update, can only place an order)

			CustomerNameBox.IsEnabled = true;
			CustomerEmailBox.IsEnabled = true;                                  //enable disabled fields to input information from customer for order placing
			CustomerAddressBox.IsEnabled = true;


			PriceBox.Text = ProductListWindow.cart.m_totalPrice.ToString();                 //set price textBox
			OrderListBox.ItemsSource = ProductListWindow.cart.Items;                        // Bind ItemList with ListBox
		}

		public OrderDetailsWindow(int ID)
		{
			InitializeComponent();

			PlaceOrderButton.Visibility = Visibility.Hidden;
			UpdateButton.Visibility = Visibility.Visible;

			FillFields(ID);
		}

		public OrderDetailsWindow(OrderForList orderForList)                                //read only window
		{
			InitializeComponent();

			PlaceOrderButton.Visibility = Visibility.Hidden;
			UpdateButton.Visibility = Visibility.Visible;

			FillFields(orderForList.m_id);

		}

		private void FillFields(int ID)                                                 //update window
		{
			Order order = bl.Order.Read(ID);

			StatusBox.ItemsSource = Enum.GetValues(typeof(Enums.OrderStatus));                   //gets list for status to display in comboBox
			OrderListBox.ItemsSource = order.Items;                                                            // Bind ItemList with the ListBox

			IDBox.Text = order.m_id.ToString();
			CustomerNameBox.Text = order.m_customerName;
			CustomerEmailBox.Text = order.m_customerEmail;                              //display info to update from order
			CustomerAddressBox.Text = order.m_customerAddress;
			PriceBox.Text = order.m_totalPrice.ToString();
		}

		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			bl.Order.UpdateOrderStatus(Int32.Parse(IDBox.Text), (Enums.OrderStatus)StatusBox.SelectedValue);                                     //send the product to be updated in the BL

			this.Close();
		}

		private void PlaceOrderButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ProductListWindow.cart.m_customerName = CustomerNameBox.Text;
				ProductListWindow.cart.m_customerEmail = CustomerEmailBox.Text;
				ProductListWindow.cart.m_customerAddress = CustomerAddressBox.Text;

				bl.Cart.PlaceOrder(ProductListWindow.cart);
				ProductListWindow.cart.Items.Clear();
				ProductListWindow.cart.m_totalPrice = 0;
				ProductListWindow.cart.m_customerName = "";
				ProductListWindow.cart.m_customerEmail = "";
				ProductListWindow.cart.m_customerAddress = "";

				this.Close();
				
			}
			catch (plException exc)
			{
				return;
			}
			catch (BO.InputIsInvalidException exc)
			{
				new ErrorWindow("Invalid Input", "The " + exc.Message + " was invalid").ShowDialog();
				return;
			}
			catch (BO.UnableToExecute exc)
			{
				new ErrorWindow("Unable to Execute", exc.Message).ShowDialog();
			}
			catch (Exception exc)
			{
				new ErrorWindow("Invalid Input", "The " + exc.Message + " is invalid").ShowDialog();
				return;
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void CustomerNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!Regex.IsMatch(e.Text, @"^[a-zA-Z\s]+$"))
				e.Handled = true;
		}

		private void CustomerEmailBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!Regex.IsMatch(e.Text, @"^[a-zA-Z0-9.@]+$") ||																//acceptable characters
				(e.Text == "." && (CustomerEmailBox.Text.Contains(".") || !CustomerEmailBox.Text.Contains("@"))) ||			//can't put . before @
				(Regex.IsMatch(e.Text, @"^[@]+$") && (CustomerEmailBox.Text.Contains("@") || CustomerEmailBox.Text == "")))	//can't put @ to start or more than once
				e.Handled = true;
		}

		private void CustomerAddressBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!Regex.IsMatch(e.Text, @"^[a-zA-Z0-9\s]+$") ||
					(Regex.IsMatch(e.Text, @"^[a-zA-Z\s]+$") && CustomerAddressBox.Text == ""))
				e.Handled = true;
		}
	}
}