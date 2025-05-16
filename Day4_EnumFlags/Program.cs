[Flags]
enum FileAccess
{
    None    = 0,
    Read    = 1,  // 0001
    Write   = 2,  // 0010
    Execute = 4,  // 0100
    All     = Read | Write | Execute
}

class Program{
    static void Main(){
        var access = FileAccess.Read | FileAccess.Write;
        Console.WriteLine(access);  // Output: Read, Write

        // Cek apakah akses memiliki Read
        if ((access & FileAccess.Read) == FileAccess.Read)
        {
            Console.WriteLine("Read access granted.");
        }
        else{
            Console.WriteLine("Invalid");
        }
    }
}
