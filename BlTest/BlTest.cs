using BlApi;
using BlImplementation;
using Dal;
namespace BlTest;
class BlTest
{
    static IBl bl = new Bl();
    static void Main(string[] args)
    {
		int choice;
		do
		{
			Console.WriteLine("0 - Exit\n" +
				"Who are you?\n" +
				"1 - I am a manager\n" +
				"2 - I am a customer");

			choice = Convert.ToInt16(Console.ReadLine());
			try
			{
				switch (choice)
				{
					case 0:
						Console.WriteLine("Have a good day!\n");
						break;

					case 1:
						ManagerChosen();
						break;

					case 2:
						CustomerChosen();
						break;
					default:
						Console.WriteLine("Please choose a number from the above list.");
						continue;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

		} while (choice != 0);
		return;
	}

	static void ManagerChosen()
	{
		char subChoice = '-';
		do
		{
			Console.WriteLine("Manager Menu\n" +
						"What would you like to do?\n" +
						"a - View list of products\n" +
						"b - Single product details\n" +
						"c - Add a product\n" +
						"d - Delete a product\n" +
						"e - Update a  product\n" +
						"f - View list of orders\n" +
						"g - Single order details\n" +
						"h - Update shipped order\n" +
						"i - Update delivered order\n" +
						"j - Track an order\n" +
						"x - Return to Main Menu");
			subChoice = Console.ReadLine().First();

			switch (subChoice)
			{
				case 'x':
					Console.WriteLine("Goodbye...\n");
					break;

				case 'a':

					break;

				case 'b':

					break;

				case 'c':

					break;

				case 'd':

					break;

				case 'e':

					break;

				case 'f':

					break;

				case 'g':

					break;

				case 'h':

					break;

				case 'i':

					break;

				case 'j':

					break;

				default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
			}

		} while (subChoice != 'x');

	}
	static void CustomerChosen()
	{
		char subChoice = '-';
		do
		{
			Console.WriteLine("Product Menu\n" +
						"What would you like to do?\n" +
						"a - View list of products\n" +
						"b - Single product details\n" +
						"c - Add product to cart\n" +
						"d - Update quantity or product in cart\n" +
						"e - Place the order\n" +
						"f - View order details\n" +
						"x - Return to Main Menu");
			subChoice = Console.ReadLine().First();

			switch (subChoice)
			{
				case 'x':
					Console.WriteLine("Goodbye...\n");
					break;

				case 'a':

					break;

				case 'b':

					break;

				case 'c':

					break;

				case 'd':

					break;

				case 'e':

					break;

				case 'f':

					break;
				default:
					Console.WriteLine("Please choose a letter from the above list.");
					break;
			}

		} while (subChoice != 'x');
	}
}
