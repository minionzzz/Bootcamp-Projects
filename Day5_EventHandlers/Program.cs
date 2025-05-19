public delegate void NumberProcessedHandler(int number);

public class NumberProcessor
{
    public event NumberProcessedHandler OnNumberProcessed;

    public void Process(int number)
    {
        Console.WriteLine($"Angka yang diproses: {number}");
        OnNumberProcessed?.Invoke(number);
    }
}

public class NumberSubscriber
{
    public void TampilkanKuadrat(int number)
    {
        Console.WriteLine($"Kuadrat: {number * number}");
    }

    public void TampilkanPangkatTiga(int number)
    {
        Console.WriteLine($"Pangkat 3: {number * number * number}");
    }

    public void CekGenapGanjil(int number)
    {
        string status = (number % 2 == 1) ? "Ganjil" : "Genap";
        Console.WriteLine($"Angka ini adalah: {status}");
    }
}

class Program
{
    static void Main()
    {
        NumberProcessor processor = new NumberProcessor();
        NumberSubscriber subscriber = new NumberSubscriber();

        // Tambahkan metode ke event (multicast)
        processor.OnNumberProcessed += subscriber.TampilkanKuadrat;
        processor.OnNumberProcessed += subscriber.TampilkanPangkatTiga;
        processor.OnNumberProcessed += subscriber.CekGenapGanjil;

        // Proses angka
        processor.Process(8);

        Console.WriteLine();
        processor.Process(2);
    }
}
