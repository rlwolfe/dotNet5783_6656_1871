using BO;
using System;
using System.Collections.Generic;
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

namespace PL.Orders
{
	/// <summary>
	/// Interaction logic for OrderTrackerWindow.xaml
	/// </summary>
	public partial class OrderTrackerWindow : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();
		public OrderTrackerWindow(int ID)
		{
			InitializeComponent();
			IDTextBox.Text = ID.ToString();
			OrderTrackingText.Text = bl.Order.OrderTracker(ID).ToString();
		}

		private void ViewOrderButton_Click(object sender, RoutedEventArgs e) => new OrderDetailsWindow(int.Parse(IDTextBox.Text)).Show();

		private void CloseButton_Click(object sender, RoutedEventArgs e) => this.Close();
	}
}
