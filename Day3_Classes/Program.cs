using System;
using System.Reflection;
using System.Collections;
    
public class Person
{
    public string Nama { get; set; }
    public string Age { get; set; }
}

class Program
{
    static void Main()
    {
        List<Person> people = new List<Person>{
            new Person { Nama = "Budi", Age = "25" },
            new Person { Age = "20" },
            new Person { Nama = "Siti", Age = "30" }
        };

        foreach (var p in people)
        {
            PropertyInfo[] properties = p.GetType().GetProperties();

            Console.WriteLine("---- Person ----");
            
            foreach (var pro in properties)
            {
                Console.WriteLine($"{pro.Name}: {pro.GetValue(p)}");
                
            }
            Console.WriteLine();
        }
    }
}

