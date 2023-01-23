using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BO
{
	public class Cart : INotifyPropertyChanged
	{
		public string? m_customerName { get; set; }
		public string? m_customerEmail { get; set; }
		public string? m_customerAddress { get; set; }
		public double m_totalPrice { get; set; }

		private ObservableCollection<OrderItem?> m_items = new ObservableCollection<OrderItem?>();
		public ObservableCollection<OrderItem?> Items
		{
			get => m_items;
			set
			{
				m_items = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("Items"));
				}
			}
		}
		public event PropertyChangedEventHandler? PropertyChanged;

		public override string ToString()
		{
			string str = m_items.Select(item => item.ToString() + "\n").ToString();
			/*foreach (OrderItem item in m_items)                 //creates string to add to ToString of list items in cart
			{
				str += item.ToString();
				str += "\n";
			};*/
			if (str == "")
				str = "none";

			return $@"Customer's Name - {m_customerName},
			Customer's Email - {m_customerEmail},
			Customer's Address - {m_customerAddress},
			Total Price: {m_totalPrice},

			List of Items in cart: {str}";
		}
	}
}