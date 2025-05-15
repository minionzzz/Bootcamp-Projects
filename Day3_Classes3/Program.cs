// using System;
// using System.Collections.Generic;
// using System.Linq;

public class Person
{
    public string Nama { get; set; }
    public int Age { get; set; }
}

class Program
{
    static void Main()
    {
        List<Person> people = new List<Person>();

        Console.WriteLine("=== Input Data Orang ===");

        while (true)
        {
            Console.Write("Masukkan Nama (atau ketik 'selesai' untuk berhenti): ");
            string nama = Console.ReadLine();

            if (nama.ToLower() == "selesai"){
                break;
            }
                
            Console.Write("Masukkan Umur: ");
            string umurInput = Console.ReadLine();

            if (int.TryParse(umurInput, out int umur))
            {
                people.Add(new Person { Nama = nama, Age = umur });
            }
            else
            {
                Console.WriteLine("Umur tidak valid. Coba lagi.");
            }

            Console.WriteLine();
        }

        // Tampilkan hasil
        Console.WriteLine("\n=== Semua Data ===");
        foreach (var p in people)
        {
            Console.WriteLine($"{p.Nama}, {p.Age}");
        }

        // LINQ: filter umur > 20
        var dewasa = people.Where(p => p.Age > 20);

        Console.WriteLine("\n=== Orang dengan umur > 20 ===");
        foreach (var p in dewasa)
        {
            Console.WriteLine($"{p.Nama}, {p.Age}");
        }

        // LINQ Urut nama Descending
        var urutNama = people.OrderByDescending(p => p.Nama);
        Console.WriteLine("\n=== Order by Name Desc ===");
        foreach (var name in urutNama)
        {
            Console.WriteLine($"{name.Nama}: {name.Age}");
        }
    }
}
