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

namespace PL.Popups
{
	/// <summary>
	/// Interaction logic for SuccessWindow.xaml
	/// </summary>
	public partial class SuccessWindow : Window
	{
		public SuccessWindow(string successMessage)
		{
			InitializeComponent();
			SuccessMessage.Text = successMessage;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e) => Close();
	}
}
