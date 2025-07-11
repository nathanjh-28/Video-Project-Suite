namespace Video_Project_Suite.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // test the calculator class to show that test project is connected to the api project
        var calc = new Video_Project_Suite.Api.calculator();
        Assert.Equal(3, calc.Add(1, 2));
        Assert.Equal(1, calc.Subtract(3, 2));
        Assert.Equal(6, calc.Multiply(2, 3));
        Assert.Equal(2, calc.Divide(6, 3));
        Assert.Throws<DivideByZeroException>(() => calc.Divide(1, 0));
    }
}
