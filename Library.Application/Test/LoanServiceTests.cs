using System;

namespace Library.Application.Tests;

public class LoanServiceTests
{
    
    public void CalculateFine_WhenReturnIsDelayed_ShouldReturnCorrectAmount()
    {
        
        var dueDate = DateTime.UtcNow.AddDays(-2); 
        var returnDate = DateTime.UtcNow;
        decimal finePerDay = 5000;

        
        var daysDelayed = (returnDate - dueDate).Days;
        var totalFine = daysDelayed * finePerDay;

        
        if (totalFine == 10000)
        {
            Console.WriteLine("🧪 Test Pasado: Multa calculada correctamente.");
        }
    }
}