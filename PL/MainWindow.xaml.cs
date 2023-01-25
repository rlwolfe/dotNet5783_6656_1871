using PL.Orders;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace PL
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();
		public MainWindow()
		{
			InitializeComponent();
		}

		private void AdminButton_Click(object sender, RoutedEventArgs e) => new AdminWindow().Show();

		private void NewOrderButton_Click(object sender, RoutedEventArgs e) => new Products.ProductListWindow(sender).Show();

		private void OrderTrackingButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (int.TryParse(OrderIDTextBox.Text, out int ID))
				{
					if (bl.Order.Read(ID) != null)
						new OrderTrackerWindow(ID).Show();
				}
				else
				{
					new ErrorWindow("Error", "Please enter an order ID").Show();
					OrderIDTextBox.Text = "";
				}
			}
			catch (Exception exc)
			{
				new ErrorWindow("Invalid Entry", $"ID entered doesn't match any current {exc.Message}s").Show();
			}
		}

		private void OrderIDTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			if(!Regex.IsMatch(e.Text, @"^[0-9]+$"))
				e.Handled = true;
		}

		private void OrderIDTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (!int.TryParse(OrderIDTextBox.Text, out int result))
				OrderIDTextBox.Text = "";
		}
	}
}
