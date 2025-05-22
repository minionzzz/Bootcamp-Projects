using System.Formats.Asn1;

Console.Write("Enter a number: ");
if (int.TryParse(Console.ReadLine(), out int n))
{
    for (int x = 1; x <= n; x++)
    {
        if (x % 3 == 0 && x % 5 == 0 && x % 7 == 0)
        {
            Console.Write("foobarjazz, ");
        }
        else if (x % 3 == 0 && x % 7 == 0)
        {
            Console.Write("foojazz, ");
        }
        else if (x % 5 == 0 && x % 7 == 0)
        {
            Console.Write("barjazz, ");
        }
        else if (x % 3 == 0 && x % 5 == 0)
        {
            Console.Write("foobar, ");
        }
        else if (x % 3 == 0)
        {
            Console.Write("foo, ");
        }
        else if (x % 5 == 0)
        {
            Console.Write("bar, ");
        }
        else if (x % 7 == 0)
        {
            Console.Write("jazz, ");
        }
        else
        {
            Console.Write(x + ", ");
        }
    }
}
else
{
    Console.WriteLine("Invalid input. Please enter a valid integer.");
}
