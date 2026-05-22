namespace Library.Domain.Entities;

public class Copy
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}