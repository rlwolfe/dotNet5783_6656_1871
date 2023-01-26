using BO;
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
using System.Windows.Markup;
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
			AscRadioButton.IsChecked = true;
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

		private void GroupingButton_Click(object sender, RoutedEventArgs e)
		{
			var groupedOrders = from order in bl.Order.ReadAll()
								group order by order.m_status.ToString();

			OrdersListView.ItemsSource = from item in groupedOrders
										 from i in item
										 select new KeyValuePair<String, OrderForList>(item.Key.ToUpper() + ":\n", i);
		}

		private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (AscRadioButton != null)			//if not coming for initialization 
				SortList();
		}

		private void AscRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			SortList();
		}

		private void DescRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			SortList();
		}
		
		private void SortList()
		{
			if ((bool)AscRadioButton.IsChecked)						//sort by ascending
			{
				switch (SortComboBox.SelectedIndex)
				{
					case 0:     //ID
						OrdersListView.ItemsSource = bl.Order.ReadAll().OrderBy(order => order.m_id);
						break;
					case 1:     //customer name
						OrdersListView.ItemsSource = bl.Order.ReadAll().OrderBy(order => order.m_customerName);
						break;
					case 2:     //price
						OrdersListView.ItemsSource = bl.Order.ReadAll().OrderBy(order => order.m_totalPrice);
						break;
					case 3:     //# of items
						OrdersListView.ItemsSource = bl.Order.ReadAll().OrderBy(order => order.m_amountOfItems);
						break;
					default:
						break;
				}
			}
			else											//sort by descending
			{
				switch (SortComboBox.SelectedIndex)
				{
					case 0:     //ID
						OrdersListView.ItemsSource = bl.Order.ReadAll().OrderByDescending(order => order.m_id);
						break;
					case 1:     //customer name
						OrdersListView.ItemsSource = bl.Order.ReadAll().OrderByDescending(order => order.m_customerName);
						break;
					case 2:     //price
						OrdersListView.ItemsSource = bl.Order.ReadAll().OrderByDescending(order => order.m_totalPrice);
						break;
					case 3:     //# of items
						OrdersListView.ItemsSource = bl.Order.ReadAll().OrderByDescending(order => order.m_amountOfItems);
						break;
					default:
						break;
				}
			}
		}
	}
}
