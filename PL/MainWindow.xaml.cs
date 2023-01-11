using BlApi;
using BlImplementation;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IBl bl = new Bl();
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ShowProductsButton_Click(object sender, RoutedEventArgs e) => new Products.ProductListWindow().Show();
		private void ShowOrdersButton_Click(object sender, RoutedEventArgs e) => new Orders.OrderListWindow().Show();
		private void ShowCartsButton_Click(object sender, RoutedEventArgs e) => new Carts.CartWindow().Show();

    }
}
