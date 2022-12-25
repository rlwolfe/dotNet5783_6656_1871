﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
	public interface IDal
	{
		IOrder Order { get; }
		IProduct Product { get; }
		IOrderItem OrderItem { get; }
	}
}
