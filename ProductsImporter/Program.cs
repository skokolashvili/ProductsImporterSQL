internal class Program
{
    static void Main(string[] args)
    {
        string filePath = "D:\\Products.txt";
        var catalogue = FileDataLoader.GetCategories(filePath);
        DatabaseImporter.ImportCatalogue(catalogue);

        foreach (var category in catalogue)
        {
            Console.WriteLine($"{category.CategoryName}, {category.Products.Count}");
        }
    }
}
