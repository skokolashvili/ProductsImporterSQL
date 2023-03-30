

using System.Diagnostics.CodeAnalysis;

public sealed class Category
{
    public string? CategoryName { get; set; }
    public string? CategoryCode { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public override bool Equals(object? obj)
    {
        if (obj == null || (obj is not Category)) { return false; }

        Category other = (Category)obj;
        return CategoryCode == other.CategoryCode;
    }

    public override int GetHashCode()
    {
        return CategoryCode.GetHashCode();
    }
}
