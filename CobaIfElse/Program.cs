
Console.WriteLine("Masukan Umur: ");
int.TryParse(Console.ReadLine(), out int a);

if(a>=1 && a <= 10){
    Console.WriteLine("Anak-Anak");
}
else if(a>10 && a<17){
    Console.WriteLine("Remaja");
}
else{
    Console.WriteLine("Dewasa");
}

