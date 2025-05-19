foreach (char c in "beer")
{
    Console.WriteLine(c);
}

IEnumerable<int> Fibs(int fibCount)
{
    int prevFib = 0, curFib = 1;
    for (int i = 0; i < fibCount; i++)
    {
        yield return prevFib;
        int newFib = prevFib + curFib;
        prevFib = curFib;
        curFib = newFib;
    }
}

foreach (int fib in Fibs(6))
{
    Console.Write(fib + " ");
}