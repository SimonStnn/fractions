using System;

namespace FractionsLibrary
{
	public partial class Fraction
	{
		private int _numerator;
		private int _denominator;

		public int Numerator { get { return _numerator; } set { _numerator = value; } }
		public int Denominator
		{
			get { return _denominator; }
			set {
				if(value == 0)
					throw new DivideByZeroException();
				if(value < 0)
				{
					Numerator = -Numerator;
					value = Math.Abs(value);
				}
				_denominator = value;
			}
		}

		public Fraction() : this(1, 1) { }
		public Fraction(int numerator, int denominator)
		{
			Numerator = numerator;
			Denominator = denominator;
		}

		public Fraction Add(Fraction right)
		{
			int lcm = LCM(Denominator, right.Denominator);

			// Adjust numerators to have a common denominator
			int numerator1 = Numerator * (lcm / Denominator);
			int numerator2 = right.Numerator * (lcm / right.Denominator);

			// Add the numerators together
			int sum = numerator1 + numerator2;

			return new Fraction(sum, lcm).Simplify();
		}

		public Fraction Subtract(Fraction right)
		{
			int lcm = LCM(Denominator, right.Denominator);

			// Adjust numerators to have a common denominator
			int numerator1 = Numerator * (lcm / Denominator);
			int numerator2 = right.Numerator * (lcm / right.Denominator);

			// Subtract the numerators
			int difference = numerator1 - numerator2;

			return new Fraction(difference, lcm).Simplify();
		}

		public Fraction Multiply(Fraction right)
		{
			return new Fraction(Numerator * right.Numerator, Denominator * right.Denominator).Simplify();
		}

		public Fraction Divide(Fraction right)
		{
			return Multiply(right.Reciprocal()).Simplify();
		}

		public Fraction Reciprocal()
		{
			return new Fraction(Denominator, Numerator).Simplify();
		}

		public Fraction Invert()
		{
			return new Fraction(-Numerator, Denominator).Simplify();
		}

		public Fraction Simplify()
		{
			int gcd = GCD(Math.Abs(Numerator), Math.Abs(Denominator));
			return new Fraction(Numerator / gcd, Denominator / gcd);
		}

		public double Result()
		{
			return (double)Numerator / (double)Denominator;
		}

		// Helper method to calculate the greatest common divisor (GCD) of two numbers
		private static int GCD(int a, int b)
		{
			while(b != 0)
			{
				int temp = b;
				b = a % b;
				a = temp;
			}
			return a;
		}

		private static int LCM(int a, int b)
		{
			return Math.Abs(a * b / GCD(a, b));
		}

		public override string ToString()
		{
			return $"{Numerator}/{Denominator}";
		}

		// Equals method to check if two fractions are equal
		public override bool Equals(object other)
		{
			if(other is null)
				return false;
			else if(other is not Fraction)
				return false;

			Fraction otherFraction = (Fraction)other;

			// Simplify both fractions before comparing
			Fraction simplifiedThis = this.Simplify();
			Fraction simplifiedOther = otherFraction.Simplify();

			// Check if both numerator and denominator of the simplified fractions are equal
			return simplifiedThis.Numerator == simplifiedOther.Numerator &&
				   simplifiedThis.Denominator == simplifiedOther.Denominator;
		}

		// Override the equality operator to compare two Fraction objects
		public static bool operator ==(Fraction left, Fraction right)
		{
			if(left is null)
				return right is null;

			return left.Equals(right);
		}

		public static bool operator !=(Fraction left, Fraction right)
		{
			if(left is null)
				return right is null;

			return !left.Equals(right);
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
	}
}