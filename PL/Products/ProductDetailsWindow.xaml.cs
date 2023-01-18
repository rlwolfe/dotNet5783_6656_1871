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

namespace PL.Products
{
	/// <summary>
	/// Interaction logic for ProductDetailsWindow.xaml
	/// </summary>
	public partial class ProductDetailsWindow : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();
		public ProductDetailsWindow(ProductItem product)
		{
			InitializeComponent();
			/*AddButton.Visibility = Visibility.Hidden;                       //adding not enabled in this case
			CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));            //gets list for category display in comboBox

			IDBox.Text = product.m_id.ToString();
			NameBox.Text = product.m_name;
			CategoryBox.SelectedItem = product.m_category;                              //display info to update from product
			PriceBox.Text = product.m_price.ToString();
			InStockBox.Text = product.m_inStock.ToString();
			AmountBox.Text = product.m_amount.ToString();*/
		}
	}
}
