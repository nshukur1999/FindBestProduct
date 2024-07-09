public class Category
{
    public string? CategoryName { get; set; }
    public List<SubCategory> SubCategories { get; set; }
}

public class SubCategory
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}