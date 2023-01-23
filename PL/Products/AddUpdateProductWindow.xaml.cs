using BO;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Products
{
	/// <summary>
	/// Interaction logic for AddUpdateProduct.xaml
	/// </summary>
	public partial class AddUpdateProductWindow : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();
		public AddUpdateProductWindow()										//create a new product
		{
			InitializeComponent();
			UpdateButton.Visibility = Visibility.Hidden;				//update not enabled in this case
			IDBox.Visibility = Visibility.Hidden;						//ID not shown because it is allocated after creation
			IDLabel.Visibility = Visibility.Hidden;
			CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));            //gets list for category display in comboBox
		}
		
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{	
				bl.Product.Create(InputValidation());                 //sends product to be created through the BL with fields that have been validated
			
				this.Close();
			}
			catch (plException exc)
			{
				return;
			}
			catch (BO.InputIsInvalidException exc)
			{
				new ErrorWindow("Invalid Input", "The " + exc.Message + " entered was invalid").ShowDialog();
				return;
			}
			catch (Exception exc)
			{
				new ErrorWindow("Invalid Input", "The " + exc.Message + " is invalid").ShowDialog();
				return;
			}
		}

		public AddUpdateProductWindow(ProductForList product)						//update existing product
		{
			InitializeComponent();
			AddButton.Visibility = Visibility.Hidden;						//adding not enabled in this case
			CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));			//gets list for category display in comboBox
			
			IDBox.Text = product.m_id.ToString();
			NameBox.Text = product.m_name;
			CategoryBox.SelectedItem = product.m_category;								//display info to update from product
			PriceBox.Text = product.m_price.ToString();
			InStockBox.Text = bl.Product.ManagerRequest(product.m_id).m_inStock.ToString();
		}

		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bl.Product.Update(InputValidation());					  //send the product to be updated in the BL with valid info
			}
			catch (plException exc)
			{
				return;
			}
			catch (BO.InputIsInvalidException exc)
			{
				new ErrorWindow("Invalid Input", "The " + exc.Message + " entered was invalid").ShowDialog();
				return;
			}
			catch (Exception exc)
			{
				new ErrorWindow("Invalid Input", "The " + exc.Message + " is invalid").ShowDialog();
				return;
			}
			this.Close();
		}

		private Product InputValidation()
		{
			BO.Product product = new BO.Product();                          //create instance of product to hold changed data
			if (IDBox.Text != "-----")
				product.m_id = int.Parse(IDBox.Text);
			product.m_name = NameBox.Text;

			if (CategoryBox.SelectedIndex != -1)
				product.m_category = (BO.Enums.Category)CategoryBox.SelectedValue;
			else
				throw new plException("Category", "The category entered was invalid");

			if (double.TryParse(PriceBox.Text, out double price))
				product.m_price = price;
			else
				throw new plException("Price", "The price entered was invalid");

			if (int.TryParse(InStockBox.Text, out int inStock))
				product.m_inStock = inStock;
			else
				throw new plException("In Stock Value", "The in stock value entered was invalid");
			
			return product;																				//returns valid product
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();      //exits window

		private void PriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)				//prevents letters or symbols (besides 1 .)
		{
			if (!Regex.IsMatch(e.Text, @"^[0-9\.]+$") || (e.Text == "." && PriceBox.Text.Contains(".")))
				e.Handled = true;
		}

		private void NameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)				//prevents numbers & symbols
		{
			if (!Regex.IsMatch(e.Text, @"^[a-zA-Z\s]+$"))
				e.Handled = true;
		}

		private void InStockBox_PreviewTextInput(object sender, TextCompositionEventArgs e)				//prevents symbols & letters
		{
			if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
				e.Handled = true;
		}
	}
}
