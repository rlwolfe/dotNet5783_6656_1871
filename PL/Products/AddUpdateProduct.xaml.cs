﻿using BO;
using System;
using System.Windows;

namespace PL.Products
{
	/// <summary>
	/// Interaction logic for AddUpdateProduct.xaml
	/// </summary>
	public partial class AddUpdateProduct : Window
	{
		private BlApi.IBl? bl = BlApi.Factory.Get();
		public AddUpdateProduct()										//create a new product
		{
			InitializeComponent();
			UpdateButton.Visibility = Visibility.Hidden;				//update not enabled in this case
			IDBox.Visibility = Visibility.Hidden;						//ID not shown because it is allocated after creation
			IDLabel.Visibility = Visibility.Hidden;
			CategoryBox.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));            //gets list for category display in comboBox
		}
		
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			BO.Product product = new BO.Product();									//Creates product with all inputed variables
			product.m_name = NameBox.Text;
			product.m_category = (BO.Enums.Category)CategoryBox.SelectedValue;
			product.m_price = Double.Parse(PriceBox.Text);
			product.m_inStock = Int32.Parse(InStockBox.Text);

			bl.Product.Create(product);											//sends product to be created through the BL
			this.Close();
		}

		public AddUpdateProduct(ProductForList product)						//update existing product
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
			BO.Product product = new BO.Product();							//create instance of product to hold changed data
			product.m_id = Int32.Parse(IDBox.Text);
			product.m_name = NameBox.Text;
			product.m_category = (BO.Enums.Category)CategoryBox.SelectedValue;
			product.m_price = Double.Parse(PriceBox.Text);
			product.m_inStock = Int32.Parse(InStockBox.Text);

			bl.Product.Update(product);										//send the product to be updated in the BL
			this.Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();		//exits window
	}
}
