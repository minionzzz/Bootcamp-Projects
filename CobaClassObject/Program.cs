using System;

class Mahasiswa{
    public String Nama{ get; set; }
    public String NIM { get; set; }

    public void TampilkanData(){
        Console.WriteLine("\n === DATA MAHASISWA ====");
        Console.WriteLine($"Nama Mahasiswa: {Nama}");
        Console.WriteLine($"NIM Mahasiswa: {NIM}");
    }
}

class Program{
    static void Main(){
          Mahasiswa mhs1 = new Mahasiswa();
          Console.Write("Masukan Nama: ");
          mhs1.Nama = Console.ReadLine();
          Console.Write("Masukan NIM: ");
          mhs1.NIM = Console.ReadLine();

          mhs1.TampilkanData();

    }
  
}
