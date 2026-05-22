using System;

namespace Library.Domain.Entities;

public class Loan
{
    public int Id { get; set; }
    public int CopyId { get; set; }
    public Copy? Copy { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal FineAmount { get; set; } = 0;
}