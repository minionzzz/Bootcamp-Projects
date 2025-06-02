using System.Formats.Asn1;

Console.Write("Enter a number: ");
if (int.TryParse(Console.ReadLine(), out int n))
{
    for (int x = 1; x <= n; x++)
    {
        string output = "";

        if (x % 3 == 0) output += "foo";
        if (x % 4 == 0) output += "baz";
        if (x % 5 == 0) output += "bar";
        if (x % 7 == 0) output += "jazz";
        if (x % 9 == 0) output += "huzz";

        if (output == "")
            Console.Write($"{x}, ");
        else
            Console.Write($"{output}, ");
    }
}
else
{
    Console.WriteLine("Invalid input. Please enter a valid integer.");
}
