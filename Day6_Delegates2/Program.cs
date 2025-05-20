// Define a generic delegate
    public delegate T Transformer<T>(T arg);

// The Transform method can now handle any type of data
public class Util
{
    public static void Transform<T>(T[] values, Transformer<T> transformer)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = transformer(values[i]);
        }
    }
}

class Program{
    static void Main(){
    // A method that squares an integer
    int Square(int x) => x * x;

    // Usage with a generic delegate
    int[] values = [ 1, 2, 3 ];
    Util.Transform(values, Square);  // Works for any type T
    }
}

