using BlApi;
using BlImplementation;
using System.Windows;

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

		private void AdminButton_Click(object sender, RoutedEventArgs e) => new Products.ProductListWindow().Show();
		//private void ShowProductsButton_Click(object sender, RoutedEventArgs e) => new Products.ProductListWindow().Show();
		//private void ShowOrdersButton_Click(object sender, RoutedEventArgs e) => new Orders.OrderListWindow().Show();

    }
}
