﻿using BO;

namespace BlApi
{
	public interface ICart
	{
		public Cart AddToCart(Cart cart, int prodID);
		public OrderItem Read(int id);
		public Cart Update(Cart cart, int prodID, int amount);
		public void PlaceOrder(Cart cart);
	}
}
