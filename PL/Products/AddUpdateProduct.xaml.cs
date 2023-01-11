using BlApi;
using BlImplementation;
using BO;
using System;
using System.Windows;

namespace PL.Products
{
	/// <summary>
	/// Interaction logic for AddUpdateProduct.xaml
	/// </summary>
	public partial class AddUpdateProduct : Window
	{
		private IBl bl = new Bl();
		public AddUpdateProduct()                   //Add/create a new product
		{
			InitializeComponent();
			UpdateButton.Visibility = Visibility.Hidden;            //update not enabled in this case
			CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
			//display next ID?
		}
		
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			BO.Product product = new BO.Product();
			product.m_name = NameBox.Text;
			product.m_category = (BO.Enums.Category)CategoryBox.SelectedValue;
			product.m_price = Double.Parse(PriceBox.Text);
			product.m_inStock = Int32.Parse(InStockBox.Text);
			bl.Product.Create(product);
			this.Close();
		}

		public AddUpdateProduct(ProductForList product)     //update existing product
		{
			InitializeComponent();
			AddButton.Visibility = Visibility.Hidden;           //adding not enabled in this case
			IDBox.Text = product.m_id.ToString();
			NameBox.Text = product.m_name;
			CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
			CategoryBox.SelectedItem = product.m_category;
			PriceBox.Text = product.m_price.ToString();
			InStockBox.Text = bl.Product.ManagerRequest(product.m_id).m_inStock.ToString();
		}

		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			BO.Product product = new BO.Product();
			product.m_id = Int32.Parse(IDBox.Text);
			product.m_name = NameBox.Text;
			product.m_category = (BO.Enums.Category)CategoryBox.SelectedValue;
			product.m_price = Double.Parse(PriceBox.Text);
			product.m_inStock = Int32.Parse(InStockBox.Text);
			bl.Product.Update(product);
			this.Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
