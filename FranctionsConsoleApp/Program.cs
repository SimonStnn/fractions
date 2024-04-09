using FractionsLibrary;

namespace FranctionsConsoleApp
{
	internal class Program
	{
		static Fraction fraction = new();

		static void WriteIntroduction()
		{
			Console.WriteLine();
			Console.WriteLine("Constructors");
			Console.WriteLine("new Fraction()");
			Console.WriteLine($"'Fraction()' = {new Fraction()}");
			Console.WriteLine("new Fraction(int numerator, int denominator)");
			Console.WriteLine($"'Fraction(2, 3)' = {new Fraction(2, 3)}");
			Console.WriteLine();
			Fraction fraction = new(2, 3);
			Console.WriteLine("Methods");
			Console.WriteLine($"* fraction will refer to fraction: {fraction}");
			Console.WriteLine();
			Console.WriteLine("Mathematical operations");
			Console.WriteLine("You can add two fractions together with Fraction.Add");
			Console.WriteLine($"Example: fraction.Add(new Fraction(4, 6)); -> {fraction.Add(new Fraction(4, 6))}");
			Console.WriteLine("You can subtract two fractions with Fraction.Subtract");
			Console.WriteLine($"Example: fraction.Subtract(new Fraction(4, 6)); -> {fraction.Subtract(new Fraction(4, 6))}");
			Console.WriteLine("You can multiply two fractions with Fraction.Multiply");
			Console.WriteLine($"Example: fraction.Multiply(new Fraction(4, 6)); -> {fraction.Multiply(new Fraction(4, 6))}");
			Console.WriteLine("You can divide two fractions with Fraction.Divide");
			Console.WriteLine($"Example: fraction.Divide(new Fraction(4, 6)); -> {fraction.Divide(new Fraction(4, 6))}");
			Console.WriteLine();
			Console.WriteLine("Other operations");
			Console.WriteLine("You can switch the numenator and denominator of the fraction with Fraction.Reciprocal");
			Console.WriteLine($"Example: fraction.Reciprocal(); -> {fraction.Reciprocal()}");
			Console.WriteLine("You can invert fraction with Fraction.Invert");
			Console.WriteLine($"Example: fraction.Invert(); -> {fraction.Invert()}");
			Console.WriteLine("You can simplify fraction with Fraction.Simplify");
			Console.WriteLine($"Example: new Fraction(4, 6).Simplify(); -> {new Fraction(4, 6).Simplify()}");
			Console.WriteLine("To get the result of the fraction you can call Fraction.Result");
			Console.WriteLine($"Example: fraction.Result(); -> {fraction.Result():F3}");
			Console.WriteLine();
			Console.WriteLine("Notes");
			Console.WriteLine("Note that each method of the Fraction class will return a new instance of ");
			Console.WriteLine("the Fraction class. The object it was called on will not change.");
		}

		static void WriteFraction(Fraction fraction)
		{
			string numenatorValue = fraction.Numerator.ToString().PadLeft(3);
			string denominatorValue = fraction.Denominator.ToString().PadLeft(3);
			int width = numenatorValue.Length > denominatorValue.Length ? numenatorValue.Length : denominatorValue.Length;

			Console.WriteLine($"{numenatorValue}");
			Console.WriteLine($"{"".PadRight(width * 2 - 1, '-')}");
			Console.WriteLine($"{denominatorValue}");
		}

		static Fraction parseFraction(string frac)
		{
			frac = frac.Replace(" ", "");
			if(frac.Length == 0)
				throw new ArgumentException("Input string is empty.");

			if(!frac.Contains("/") && int.TryParse(frac, out _))
				frac += "/1"; // If no "/" present and it's parseable to int, assume it's a whole number.

			string[] parts = frac.Split('/');
			if(parts.Length != 2)
				throw new FormatException("Fraction format is incorrect.");

			if(!int.TryParse(parts[0], out int numerator) || !int.TryParse(parts[1], out int denominator))
				throw new FormatException("Numerator or denominator is not a valid integer.");

			return new Fraction(numerator, denominator);
		}

		static void DisplayCurrentFraction()
		{
			Console.WriteLine($"Current fraction is: {fraction}");
		}

		static void DisplayMenu()
		{
			DisplayCurrentFraction();
			Console.WriteLine();
			DisplayCommands();

			//Console.WriteLine();
			//Console.WriteLine("Legend:");
			//Console.WriteLine("< > : required");
			//Console.WriteLine("[ ] : optional");
		}

		static void DisplayCommands()
		{
			// Mathematical operations
			Console.WriteLine("add <fraction>\t\tAdd a fraction to the current fraction.");
			Console.WriteLine("subtract <fraction>\tSubtract a fraction to the current fraction.");
			Console.WriteLine("multiply <fraction>\tMultiply a fraction to the current fraction.");
			Console.WriteLine("divide <fraction>\tDivide a fraction to the current fraction.");
			// Fraction properties
			Console.WriteLine("numerator\t\tDisplay the numenator of the current fraction.");
			Console.WriteLine("denominator\t\tDisplay the denominator of the current fraction.");
			// Other fraction operations
			Console.WriteLine("reciprocal\t\tSwitch the numerator and denominator of the fraction.");
			Console.WriteLine("simplify\t\tSimplify the fraction.");
			Console.WriteLine("result\t\t\tWrite the result of the fraction to the console.");
			// Application operations
			Console.WriteLine("new <fraction>\t\tUpdate the current fraction.");
			Console.WriteLine("reset \t\t\tReset the current fraction back to the default (" + new Fraction() + ").");
			Console.WriteLine("clear\t\t\tClear the screen.");
			Console.WriteLine("help\t\t\tDisplay this menu.");
			Console.WriteLine("exit\t\t\tExit the application.");
		}

		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to the Fractions library");
			WriteIntroduction();

			Console.WriteLine();
			DisplayMenu();
			Console.WriteLine();

			while(true)
			{
				Console.Write($"{fraction} > ");

				string input = $"{Console.ReadLine()}".ToLower();

				string[] parts = input.Split(' ', 2); // Splitting into command and options
				string command = parts[0];
				string[] options = parts.Length > 1 ? parts[1].Split(' ') : Array.Empty<string>();
				Fraction rightFraction;
				try
				{
					switch(command)
					{
						// Mathematical operations
						case "add":
							rightFraction = parseFraction(options[0]);
							fraction = fraction.Add(rightFraction);
							break;
						case "subtract":
							rightFraction = parseFraction(options[0]);
							fraction = fraction.Subtract(rightFraction);
							break;
						case "multiply":
							rightFraction = parseFraction(options[0]);
							fraction = fraction.Multiply(rightFraction);
							break;
						case "divide":
							rightFraction = parseFraction(options[0]);
							fraction = fraction.Divide(rightFraction);
							break;

						// Fraction properties
						case "numerator":
							Console.WriteLine(fraction.Numerator);
							break;
						case "denominator":
							Console.WriteLine(fraction.Denominator);
							break;

						// Other fraction operations
						case "reciprocal":
							fraction = fraction.Reciprocal();
							break;
						case "simplify":
							fraction = fraction.Simplify();
							break;
						case "result":
							Console.WriteLine(fraction.Result());
							break;

						// Application operations
						case "new":
							rightFraction = parseFraction(options[0]);
							fraction = rightFraction;
							break;
						case "reset":
							fraction = new Fraction();
							break;
						case "clear":
							Console.Clear();
							break;
						case "help":
							DisplayMenu();
							break;
						case "exit":
							Environment.Exit(0);
							break;
						default:
							if(command == "") break;
							Console.WriteLine($"{command} is not a valid command.");
							break;

					}
				}
				catch(FormatException)
				{
					Console.WriteLine($"{options[0]} is not a valid fraction.");
					continue;
				}
				catch(ArgumentException e)
				{
					Console.WriteLine(e.Message);
				}
				catch(IndexOutOfRangeException)
				{
					Console.WriteLine("Not enough arguments.");
				}
				catch(Exception)
				{
					Console.WriteLine("Something went wrong.");
				}
			}
		}
	}
}