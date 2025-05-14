bool lanjut = true;

Console.Write("\nKALKULATOR SEDERHANA");
while(lanjut){
    Console.WriteLine("Opsi Pilihan: ");
    Console.WriteLine("1. Tambah");
    Console.WriteLine("2. Kurang");
    Console.WriteLine("3. Kali");
    Console.WriteLine("4. Bagi");
    Console.WriteLine("5. Keluar");

    Console.Write("Input Pilihan: ");
    String pilihan = Console.ReadLine();

    if(pilihan == "5"){
        lanjut = false;
        Console.WriteLine("Terimakasih");
        break;
    }

    Console.Write("Masukan Angka: ");
    double.TryParse(Console.ReadLine(), out double a);
    Console.Write("Masukan Angka: ");
    double.TryParse(Console.ReadLine(), out double b);

    switch (pilihan){
        case "1":
        Console.WriteLine($"Hasil {a+b}");
        break;
        case "2":
        Console.WriteLine($"Hasil {a-b}");
        break;
        case "3":
        Console.WriteLine($"Hasil {a*b}");
        break;
        case "4":
        if(b == 0){
            Console.WriteLine("Undefined");
        }else{
            Console.WriteLine($"Hasil {a/b}");
        }
        break;
        default:
        Console.WriteLine("Invalid Input");
        break;
    }
}