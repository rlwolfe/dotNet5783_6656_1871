﻿using BlApi;

namespace BlImplementation
{
	sealed public class Bl : IBl
	{
		public IProduct Product => new Product();

		public IOrder Order => new Order();

		public IOrderItem OrderItem => new OrderItem();
	}
}
