using System;

public class myClass
{
    private Dictionary<int, string> _rules;
    public myClass()
    {
        _rules = new Dictionary<int, string>();
    }

    public void AddRule(int input, string output)
    {
        _rules[input] = output;
    }

    public string Generate(int number)
    {
        string result = "";

        foreach (var rule in _rules)
        {
            if (number % rule.Key == 0)
            {
                result += rule.Value;
            }
        }
        return result == "" ? number.ToString() : result;
    }

    public void PrintNumber(int n)
    {
        for (int x = 1; x <= n; x++)
        {
            Console.Write($"{Generate(x)}, ");
        }
        Console.WriteLine();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        myClass obj = new myClass();
        obj.AddRule(3, "foo");
        obj.AddRule(4, "baz");
        obj.AddRule(5, "bar");
        obj.AddRule(7, "jazz");
        obj.AddRule(9, "huzz");

        Console.Write("Enter a number: ");
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            obj.PrintNumber(n);
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }
}
    