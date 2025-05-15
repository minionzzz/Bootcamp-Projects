using System;
using System.Collections.Generic;
using System.Linq;

public class Person
{
    public string Nama { get; set; }
    public int Age { get; set; }
}

class Program
{
    static void Main()
    {
        List<Person> people = new List<Person>
        {
            new Person { Nama = "Budi", Age = 25 },
            new Person { Nama = "Adit", Age = 20 },
            new Person { Nama = "Siti", Age = 30 },
            new Person { Nama = "Dewi", Age = 18 }
        };

        // 1. LINQ: Filter orang yang umurnya > 20
        var dewasa = people.Where(p => p.Age > 20);

        Console.WriteLine("=== Orang yang umurnya > 20 ===");
        foreach (var orang in dewasa)
        {
            Console.WriteLine($"{orang.Nama}, {orang.Age}");
        }

        // 2. LINQ: Ambil hanya nama semua orang
        var namaOrang = people.Select(p => p.Nama);

        Console.WriteLine("\n=== Nama semua orang ===");
        foreach (var nama in namaOrang)
        {
            Console.WriteLine(nama);
        }

        // 3. LINQ: Urutkan berdasarkan umur
        var urutUmur = people.OrderBy(p => p.Age);

        Console.WriteLine("\n=== Urutan berdasarkan umur ===");
        foreach (var p in urutUmur)
        {
            Console.WriteLine($"{p.Nama}, {p.Age}");
        }

        // 4. LINQ: Urutkan berdasarkan nama
        var urutNama = people.OrderBy(p => p.Nama);

        Console.WriteLine("\n=== Urutan berdasarkan Nama ===");
        foreach (var p in urutNama)
        {
            Console.WriteLine($"{p.Nama}, {p.Age}");
        }
    }
}
