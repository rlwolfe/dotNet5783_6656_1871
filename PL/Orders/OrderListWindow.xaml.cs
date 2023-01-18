using PL.Products;
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
	/// Interaction logic for OrderListWindow.xaml
	/// </summary>
	public partial class OrderListWindow : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();
		public OrderListWindow()
		{
			InitializeComponent();
			OrdersListView.ItemsSource = bl.Order.ReadAll();                     //create listView from orders in BL
		}

		private void OrdersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			BO.OrderForList order = (BO.OrderForList)OrdersListView.SelectedItem;       //take selection from list
			
			if (order == null)                                                                //didn't click an Item on list
				return;

			bool? window = new OrderDetailsWindow(order).ShowDialog();
			if (window.HasValue)
				OrdersListView.ItemsSource = bl?.Order.ReadAll();
		}
	}
}
