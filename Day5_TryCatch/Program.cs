int Calc(int x)
{
    return 10 / x;
}

try
{
    int y = Calc(0);
    Console.WriteLine(y);
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"x cannot be zero \n{ex}");
}
Console.WriteLine("Program completed");
