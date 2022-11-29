namespace DO;

/// <summary>
/// Struct of Enums
/// </summary>
public struct Enums
{
	public enum ProductName {	Apple, Orange, Potato, Cherries, Cucumber,	//0-4 Produce
						Milk, Yogurt, Cottage, Cheese, Butter,		//5-9 Dairy
						Sausage, Steak, Ribs, Hamburgers, Chicken,	//10-14 Meat
						Pizza, Popcicles, Fish, Ice, Chips,			//15-19 Frozen
						Oil, Salt, Sugar, Vinegar, Honey,			//20-24 Cooking
						Bagel, Pita, Rolls, Tortillas, Baguette,	//25-29 Bread
						Beans, Peaches, Corn, Pickles, Tuna,		//30-34 Canned
						Bamba, Crisps, Bisli, Cookies, Lollipops,	//35-39 Snack
						Wipes, Soap, Detergent, Tissues, Antiseptic,//40-44 Hygiene
						Plates, Cups, Bowls, Knives, Forks }		//45-49 Disposables

	public enum Category { Produce, Dairy, Meat, Frozen, Cooking, Bread, Canned, Snacks, Hygine, Disposables }

	public enum FirstName { Aaron, Abby, Liam, Olivia, Noah, Emma, Oliver, Charlotte, Gabriel, Talia, Rachel,
							Elijah, Amelia, James, Ava, William, Sophia, Benjamin, Isabella, Lucas, Mia, Henry,
							Evelyn, Theodore, Harper, Maria, Peter, Fred, Arthur, Danielle, Matthew, Hector, Robert,
							Jack, Jonathan, Jane, Sam, Sara, David, Heather}
	public enum LastName { Smith, Jones, Richardson, Gold, Silver, Stein, Goldstein, Goldberg, Jackson, Peterson,
							Steinberg, Rothberg, Brown, Miller, Davis, Baker, Williams, Tanner, Roth, Wilson, Lewis,
							Clark, Walker, Perez, Lee, Taylor, White, Cooper, Young, Martin, Thompson, Johnson, Cohen,
							Levi, Garcia, Moore, Green, Nelson, Hill, Adams}
	public enum streetType { St, Ln, Ave, Ct, Cres, Dr, Rd, Sq }
}
