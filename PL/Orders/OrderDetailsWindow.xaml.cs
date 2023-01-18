using BO;
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

		public OrderDetailsWindow(int ID)
		{
			InitializeComponent();
			FillFields(ID);
		}

		public OrderDetailsWindow(OrderForList orderForList)								//read only window
		{
			InitializeComponent();
			FillFields(orderForList.m_id);

		}

		private void FillFields(int ID)													//update window
		{
			Order order = bl.Order.Read(ID);
			
			StatusBox.ItemsSource = Enum.GetValues(typeof(Enums.OrderStatus));                   //gets list for status to display in comboBox
			OrderListBox.ItemsSource = order.m_items;                                                            // Bind ItemList with the ListBox
			
			IDBox.Text = order.m_id.ToString();
			CustomerNameBox.Text = order.m_customerName;
			CustomerEmailBox.Text = order.m_customerEmail;
			CustomerAddressBox.Text = order.m_customerAddress;
			PriceBox.Text = order.m_totalPrice.ToString();
			
			PaymentDatePicker.SelectedDate = order.m_paymentDate;
			OrderDatePicker.SelectedDate = order.m_orderDate;
			ShipDatePicker.SelectedDate = order.m_shipDate;                              //display info to update from order
			DelivDatePicker.SelectedDate = order.m_deliveryDate;
			StatusBox.SelectedItem = order.m_status;
		}

		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			bl.Order.UpdateOrderStatus(Int32.Parse(IDBox.Text), (Enums.OrderStatus)StatusBox.SelectedValue);                                     //send the product to be updated in the BL

			this.Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}