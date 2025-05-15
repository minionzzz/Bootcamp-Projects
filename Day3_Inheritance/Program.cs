public class Asset{
    public string Name;
}

public class Stock : Asset
{
    public long SharesOwned;
}

public class House : Asset
{
    public decimal Mortgage;
}

class Program{
    static void Main(){

        // Upcasting
        Stock s = new Stock { Name = "MSFT", SharesOwned = 1000 };
        Asset asst = s;  
        Console.WriteLine(as.Name); 
        Console.WriteLine();

        // Downcasting
        Asset ast = new Stock{Name = "NSFT", SharesOwned = 1000};
        Stock c = (Stock)ast;
        Console.WriteLine(c.Name);
        Console.WriteLine(c.SharesOwned);
    }
}