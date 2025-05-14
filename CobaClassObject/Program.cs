using System;

class Mahasiswa{
    public String Nama{ get; set; }
    public String NIM { get; set; }

    public void TampilkanData(){
        Console.WriteLine($"Nama: {Nama}");
        Console.WriteLine($"NIM: {NIM}");
    }
}

class Program{
    static void Main(){
          Mahasiswa mhs1 = new Mahasiswa();
          mhs1.Nama = "Andi";
          mhs1.NIM = "123456";

          mhs1.TampilkanData();

    }
  
}
