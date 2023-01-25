using BO;
using PL.Popups;
using System;
using System.Linq;
using System.Windows;

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
			DataContext = product;                                                          //binded data for product
			CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));            //gets list for category display in comboBox
			CategoryBox.SelectedItem = product.m_category;                                  //display product's category
		}

		private void AddToCartButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if ((bool)InStockCheckBox.IsChecked)
				{
					if (ProductListWindow.cart.Items.FirstOrDefault(x => x.m_productID == int.Parse(IDBox.Text)) == null)
					{
						bl.Cart.AddToCart(ProductListWindow.cart, int.Parse(IDBox.Text));
						new SuccessWindow($"{NameBox.Text} was successfully added to the cart").Show();
						this.Close();
					}
					else
						new ErrorWindow("Product Duplicate Error", $"{NameBox.Text} is already in the cart,\nUpdate quantity in 'View Cart'").Show();
				}
				else
					new ErrorWindow("Out of Stock", $"Sorry, {NameBox.Text} is not in stock").Show();
			}
			catch (Exception exc)
			{
				throw new plException("Error", "Error Occurred when attempted to add product to cart:\n" + exc.Message);
			}

		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void RemoveFromCartButton_Click(object sender, RoutedEventArgs e)
		{

        }
    }
}
