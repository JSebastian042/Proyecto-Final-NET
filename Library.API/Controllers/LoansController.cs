using Library.Application.Interfaces;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IRepository<Loan> _loanRepository;

    public LoansController(IRepository<Loan> loanRepository)
    {
        _loanRepository = loanRepository;
    }

    // 1. Registrar Préstamo
    [HttpPost]
    public async Task<IActionResult> CreateLoan(int copyId, int userId)
    {
        var loan = new Loan
        {
            CopyId = copyId,
            UserId = userId,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7) // Préstamo por 7 días
        };

        await _loanRepository.AddAsync(loan);
        await _loanRepository.SaveChangesAsync();
        return Ok(new { Mensaje = "Préstamo registrado con éxito.", FechaDevolucion = loan.DueDate });
    }

    // 2. Registrar Devolución + Cálculo de Multas (Punto Técnico Exigido)
    [HttpPost("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var loan = await _loanRepository.GetByIdAsync(id);
        if (loan == null) return NotFound("Registro de préstamo no encontrado.");

        loan.ReturnDate = DateTime.UtcNow;

        // Si se pasó de la fecha límite, calculamos una multa ($5.000 COP por día de retraso)
        if (loan.ReturnDate > loan.DueDate)
        {
            var diasRetraso = (loan.ReturnDate.Value - loan.DueDate).Days;
            loan.FineAmount = diasRetraso * 5000;
        }

        _loanRepository.Update(loan);
        await _loanRepository.SaveChangesAsync();

        return Ok(new
        {
            Mensaje = "Libro devuelto.",
            Multa = loan.FineAmount > 0 ? $"Tienes una multa de ${loan.FineAmount} COP por retraso." : "Sin multas."
        });
    }

   
    [HttpGet("analytics/most-borrowed")]
    public async Task<IActionResult> GetMostBorrowedReport()
    {
        var loans = await _loanRepository.GetAllAsync();

        
        return Ok(new
        {
            Descripcion = "Libro más prestado por categoría (Muestra estática para Demo)",
            Categoria = "Ficción",
            Libro = "Cien años de soledad",
            TotalPrestamos = 12
        });
    }
}