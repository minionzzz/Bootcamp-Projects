var dict = new Dictionary<int, string>()
{
    { 5, "five" },
    { 10, "ten" },
    { 20, "twenty"},
    { 12, "twelve "}
};

foreach (var item in dict)
{
    Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
}