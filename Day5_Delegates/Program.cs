// 1. Mendeklarasikan delegate
public delegate double OperasiMatematika(double a, double b);

// 2. Metode yang cocok dengan signature delegate
public class Kalkulator
{
    public static double Tambah(double a, double b) => a + b;
    public static double Kurang(double a, double b) => a - b;
    public static double Kali(double a, double b) => a * b;
    public static double Bagi(double a, double b) => a / b;
}

// 3. Menggunakan delegate
class Program
{
    static void Main()
    {
        OperasiMatematika operasi = Kalkulator.Tambah;
        Console.WriteLine(operasi(10, 5)); // Output: 15

        operasi += Kalkulator.Kurang;
        Console.WriteLine(operasi(10, 5)); // Output: 5

        operasi += Kalkulator.Kali;
        Console.WriteLine(operasi(10, 5)); // Output: 50

        operasi += Kalkulator.Bagi;
        Console.WriteLine(operasi(10, 5)); // Output: 2

        Console.WriteLine();
        // Using Func
        Func<int, double, int, double> add = (x, y, z) => x + y + z;
        Console.WriteLine(add(2, 3.5, 5));

        // Using Action
        Action<int, int> tambah = (a, b) => Console.WriteLine(a += b);
        tambah(3, 3);  

        Func<string, int> getLength = s => s.Length;
        Console.WriteLine(getLength("Namaku Alvin"));
    }
}



