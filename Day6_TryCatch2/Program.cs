// Convert String to int
string input = "123";
int result;
bool success = int.TryParse(input, out result);

if (success)
{
    Console.WriteLine($"Parsed successfully: {result}");
}
else
{
    Console.WriteLine("Failed to parse the input.");
}


//Convert int to String
int masukan = 123;
string hasil = masukan.ToString();

if (hasil.Contains("123"))
{
    Console.WriteLine($"Parsed successfully: {result}");
}
else
{
    Console.WriteLine("Failed to parse the input.");
}