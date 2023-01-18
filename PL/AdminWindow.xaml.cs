﻿using System;
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

namespace PL
{
	/// <summary>
	/// Interaction logic for AdminWindow.xaml
	/// </summary>
	public partial class AdminWindow : Window
	{
		public AdminWindow()
		{
			InitializeComponent();
		}

		private void ShowProductsButton_Click(object sender, RoutedEventArgs e) => new Products.ProductListWindow(sender).Show();

		private void ShowOrdersButton_Click(object sender, RoutedEventArgs e) => new Orders.OrderListWindow().Show();
	}
}
