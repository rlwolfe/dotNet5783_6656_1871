// See https://aka.ms/new-console-template for more information
using System;

namespace Stage0
{
	class Program
	{
		static void Main(string[] args)
		{
			Welcome6656(); Console.ReadKey();
		}

		private static void Welcome6656()
		{
			Console.WriteLine("Enter your name: ");
			string? name = Console.ReadLine();
			Console.WriteLine("{0}, welcome to my first console application", name);  //($"{name}, welcome to my first console application");
		}
	}
}
