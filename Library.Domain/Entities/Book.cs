namespace Library.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    
    public string Isbn { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public List<Author> Authors { get; set; } = new();
}