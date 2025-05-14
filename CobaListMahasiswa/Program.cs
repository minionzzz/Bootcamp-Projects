using System;
using System.Collections.Generic;

class Mahasiswa{
    public String Nama{ get; set; }
    public String NIM { get; set; }

    public void TampilkanData(){
        Console.WriteLine($"Nama Mahasiswa: {Nama}");
        Console.WriteLine($"NIM Mahasiswa: {NIM}");
    }
}


class Program{
    public static void Main(){
          Console.Write("Input Jumlah Mahasiswa: ");
          int.TryParse(Console.ReadLine(), out int total);

          List<Mahasiswa> daftarMahasiswa = new List<Mahasiswa>();

          Console.WriteLine("\t === DATA MAHASISWA ====");

          for (int i = 0; i < total; i++)
          {
            Mahasiswa mhs = new Mahasiswa();
            Console.Write("Input Nama: ");
            mhs.Nama = Console.ReadLine();

            Console.Write("Input NIM: ");
            mhs.NIM = Console.ReadLine();

            daftarMahasiswa.Add(mhs);
          }

        Console.WriteLine("\t === LIST MAHASISWA ===");
          foreach (Mahasiswa i in daftarMahasiswa)
          {
            i.TampilkanData();
          }

    }
  
}