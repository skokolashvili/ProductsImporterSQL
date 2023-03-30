public static class FileDataLoader
{
    public static IEnumerable<Category> GetCategories(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));
        if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

        var data = File.ReadAllLines(filePath);
        ValidateData(data);
        List<Category> categories = new List<Category>();

        foreach (var line in data)
        {
            string[] values = line.Split('\t');
            Category category = new Category();         
            category.CategoryName = values[0];
            category.CategoryCode = values[1];
            category.IsDeleted = IsDeleted(values[2]);
            categories.Add(category);
        }
        List<Category> uniqueCategories = categories.Distinct().ToList();

        foreach (var categorie in uniqueCategories)
        {
            foreach (var line in data)
            {
                Product product = new Product();
                string[] values = line.Split('\t');
                if (categorie.CategoryName == values[0])
                {
                    product.ProductName = values[3];
                    product.ProductCode = values[4];
                    product.ProductPrice = Convert.ToDouble(values[5]);
                    product.IsDeleted = IsDeleted(values[6]);

                    categorie.Products.Add(product);
                }
            }
        }
        return uniqueCategories;
    }
    private static bool IsDeleted(string isDeleted)
    {
        if (Convert.ToInt32(isDeleted) == 0 )
        {
            return false;
        }
        return true;
    }

    private static void ValidateData(string[] data)
    {
        List<Exception> exceptions = new();
        for (int i = 0; i < data.Length; i++)
        {
            string[] columns = data[i].Split('\t');
            if (columns.Length != 7) exceptions.Add(new FormatException("Incorect Format"));
            if (!int.TryParse(columns[1], out int categoryCode)) exceptions.Add(new FormatException("Incorect Format"));
            if (int.Parse(columns[2]) != 0 && int.Parse(columns[2]) != 1) exceptions.Add(new FormatException("Incorect Format"));
            if (!int.TryParse(columns[4], out int productCode)) exceptions.Add(new FormatException("Incorect Format"));
            if (!double.TryParse(columns[5], out double price)) exceptions.Add(new FormatException("Incorect Format"));
            if (int.Parse(columns[6]) != 0 && int.Parse(columns[6]) != 1) exceptions.Add(new FormatException("Incorect Format"));
        }
        if (exceptions.Count > 0)
        {
            throw new AggregateException(exceptions);
        }
    }
}
