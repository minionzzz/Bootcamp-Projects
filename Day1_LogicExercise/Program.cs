        Console.Write("Enter a number: ");
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            for (int x = 1; x <= n; x++)
            {
                if (x % 3 == 0 && x % 5 == 0)
                {
                    Console.Write("foobar ");
                }
                else if (x % 3 == 0)
                {
                    Console.Write("foo ");
                }
                else if (x % 5 == 0)
                {
                    Console.Write("bar ");
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