using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
	static public class Factory
	{
		static public IBl? Get() => new BlImplementation.Bl();
	}
}
