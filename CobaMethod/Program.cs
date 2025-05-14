using System;

static double HitungLuasPersegi(double sisi){
    return sisi * sisi;
}

Console.Write("Masukan Sisi: ");
double.TryParse(Console.ReadLine(), out double s);

double luas = HitungLuasPersegi(s);

Console.WriteLine($"Luas {luas}");
