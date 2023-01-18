using PL.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
	[Serializable]
	internal class plException : Exception
	{
		public plException(string type, string errorMessage) : base()
		{
			new ErrorWindow(type, errorMessage).Show();
		}
	}
}
