using FractionsLibrary;

namespace TestFractionLibrary
{
    public class UnitTestFraction
    {
        Fraction fraction = new();

        [Fact]
        public void TestConstructor()
        {
            Assert.Equal(1, fraction.Numerator);
            Assert.Equal(1, fraction.Denominator);

            fraction = new(2, 4);
            Assert.Equal(2, fraction.Numerator);
            Assert.Equal(4, fraction.Denominator);

            fraction = new(-3, 5);
            Assert.Equal(-3, fraction.Numerator);
            Assert.Equal(5, fraction.Denominator);

            fraction = new(3, -5);
            Assert.Equal(-3, fraction.Numerator);
            Assert.Equal(5, fraction.Denominator);

            fraction = new(-3, -5);
            Assert.Equal(3, fraction.Numerator);
            Assert.Equal(5, fraction.Denominator);
        }

        [Fact]
        public void TestProperties()
        {
            fraction.Numerator = 2;
            fraction.Denominator = 3;
            Assert.Equal(2, fraction.Numerator);
            Assert.Equal(3, fraction.Denominator);

            fraction.Numerator = -2;
            fraction.Denominator = 4;
            Assert.Equal(-2, fraction.Numerator);
            Assert.Equal(4, fraction.Denominator);

            fraction.Numerator = 3;
            fraction.Denominator = -4;
            Assert.Equal(-3, fraction.Numerator);
            Assert.Equal(4, fraction.Denominator);

            fraction.Numerator = -3;
            fraction.Denominator = -4;
            Assert.Equal(3, fraction.Numerator);
            Assert.Equal(4, fraction.Denominator);

            Assert.Throws<DivideByZeroException>(() => fraction.Denominator = 0);
        }

        [Fact]
        public void TestAdd()
        {
            Assert.Equal(new Fraction(7, 5), new Fraction(3, 5).Add(new Fraction(4, 5)));
            Assert.Equal(new Fraction(4, 5), new Fraction(2, 5).Add(new Fraction(2, 5)));
            Assert.Equal(new Fraction(5, 6), new Fraction(1, 2).Add(new Fraction(1, 3)));
            Assert.Equal(new Fraction(1, 6), new Fraction(1, 2).Add(new Fraction(-1, 3)));
            Assert.Equal(new Fraction(-1, 1), new Fraction(1, 1).Add(new Fraction(-2, 1)));
            Assert.Equal(new Fraction(-1, 1), new Fraction(3, 1).Add(new Fraction(-4, 1)));
        }

        [Fact]
        public void TestSubtract()
        {
            Assert.Equal(new Fraction(-1, 5), new Fraction(3, 5).Subtract(new Fraction(4, 5)));
            Assert.Equal(new Fraction(0, 5), new Fraction(2, 5).Subtract(new Fraction(2, 5)));
            Assert.Equal(new Fraction(1, 6), new Fraction(1, 2).Subtract(new Fraction(1, 3)));
            Assert.Equal(new Fraction(5, 6), new Fraction(1, 2).Subtract(new Fraction(-1, 3)));
            Assert.Equal(new Fraction(0, 1), new Fraction(1, 2).Subtract(new Fraction(2, 4)));
        }

        [Fact]
        public void TestMultiply()
        {
            Assert.Equal(new Fraction(12, 25), new Fraction(3, 5).Multiply(new Fraction(4, 5)));
            Assert.Equal(new Fraction(4, 25), new Fraction(2, 5).Multiply(new Fraction(2, 5)));
            Assert.Equal(new Fraction(1, 6), new Fraction(1, 2).Multiply(new Fraction(1, 3)));
            Assert.Equal(new Fraction(-1, 6), new Fraction(1, 2).Multiply(new Fraction(-1, 3)));
            Assert.Equal(new Fraction(2, 8), new Fraction(1, 2).Multiply(new Fraction(2, 4)));
        }

        [Fact]
        public void TestDevide()
        {
            Assert.Equal(new Fraction(15, 20), new Fraction(3, 5).Divide(new Fraction(4, 5)));
            Assert.Equal(new Fraction(10, 10), new Fraction(2, 5).Divide(new Fraction(2, 5)));
            Assert.Equal(new Fraction(3, 2), new Fraction(1, 2).Divide(new Fraction(1, 3)));
            Assert.Equal(new Fraction(3, -2), new Fraction(1, 2).Divide(new Fraction(-1, 3)));
            Assert.Equal(new Fraction(4, 4), new Fraction(1, 2).Divide(new Fraction(2, 4)));
        }

        [Fact]
        public void TestReciprocal()
        {
            Assert.Equal(new Fraction(1, 1), new Fraction(1, 1).Reciprocal());
            Assert.Equal(new Fraction(5, 5), new Fraction(5, 5).Reciprocal());
            Assert.Equal(new Fraction(2, 1), new Fraction(1, 2).Reciprocal());
            Assert.Equal(new Fraction(3, 1), new Fraction(1, 3).Reciprocal());
            Assert.Equal(new Fraction(3, 2), new Fraction(2, 3).Reciprocal());
        }

        [Fact]
        public void TestInvert()
        {
            Assert.Equal(new Fraction(-1, 1), new Fraction(1, 1).Invert());
            Assert.Equal(new Fraction(-5, 5), new Fraction(5, 5).Invert());
            Assert.Equal(new Fraction(-1, 2), new Fraction(1, 2).Invert());
            Assert.Equal(new Fraction(1, 3), new Fraction(-1, 3).Invert());
            Assert.Equal(new Fraction(-2, 3), new Fraction(-2, -3).Invert());
        }

        [Fact]
        public void TestSimplify()
        {
            Assert.Equal(new Fraction(1, 1), new Fraction(4, 4).Simplify());
            Assert.Equal(new Fraction(1, 1), new Fraction(8, 8).Simplify());
            Assert.Equal(new Fraction(1, 2), new Fraction(2, 4).Simplify());
            Assert.Equal(new Fraction(1, 2), new Fraction(3, 6).Simplify());
            Assert.Equal(new Fraction(2, 3), new Fraction(6, 9).Simplify());
            Assert.Equal(new Fraction(2, 3), new Fraction(30, 45).Simplify());
            Assert.Equal(new Fraction(-2, 1), new Fraction(-2, 1).Simplify());
            Assert.Equal(new Fraction(-3, 1), new Fraction(-3, 1).Simplify());
            Assert.Equal(new Fraction(-3, 1), new Fraction(3, -1).Simplify());
            Assert.Equal(new Fraction(31, 45), new Fraction(31, 45).Simplify());
            Assert.Equal(new Fraction(-1, 1), new Fraction(10, -10).Simplify());
            Assert.Equal(new Fraction(-1, 2), new Fraction(2, -4).Simplify());
            Assert.Equal(new Fraction(-1, 2), new Fraction(-2, 4).Simplify());
            Assert.Equal(new Fraction(-1, 2), new Fraction(-2, 4).Simplify());
            Assert.Equal(new Fraction(1, 2), new Fraction(-2, -4).Simplify());
        }

        [Fact]
        public void TestResult()
        {
            Assert.Equal(0, new Fraction(0, 1).Result());
            Assert.Equal(1, new Fraction().Result());
            Assert.Equal(0.5, new Fraction(1, 2).Result());
            Assert.Equal(1.5, new Fraction(3, 2).Result());
            Assert.Equal(0.667, new Fraction(2, 3).Result(), 3);
            Assert.Equal(0.1, new Fraction(1, 10).Result());
            Assert.Equal(0.25, new Fraction(1, 4).Result());
            Assert.Equal(-1, new Fraction(-1, 1).Result());
            Assert.Equal(-0.5, new Fraction(-1, 2).Result());
            Assert.Equal(-1.5, new Fraction(-3, 2).Result());
            Assert.Equal(-10, new Fraction(-20, 2).Result());
            Assert.Equal(0.123456789, new Fraction(123456789, 1000000000).Result());
        }

        [Fact]
        public void TestToString()
        {
            Assert.Equal("1/1", new Fraction(1, 1).ToString());
            Assert.Equal("1/2", new Fraction(1, 2).ToString());
            Assert.Equal("-1/2", new Fraction(-1, 2).ToString());
            Assert.Equal("3/2", new Fraction(3, 2).ToString());
            Assert.Equal("-3/2", new Fraction(-3, 2).ToString());
            Assert.Equal("0/1", new Fraction(0, 1).ToString());
            Assert.Equal("2/1", new Fraction(2, 1).ToString());
            Assert.Equal("5/5", new Fraction(5, 5).ToString());
            Assert.Equal("-7/3", new Fraction(-7, 3).ToString());
            Assert.Equal("123456789/1000000000", new Fraction(123456789, 1000000000).ToString());
        }
    }
}