using Library.Application.Interfaces;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IRepository<Book> _bookRepository;

    public BooksController(IRepository<Book> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    // GET: api/books (Con Paginación y Filtrado Complejo por Título/ISBN)
    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var books = await _bookRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(search))
        {
            books = books.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                     b.Isbn.Contains(search));
        }

        
        var paginatedBooks = books.Skip((page - 1) * pageSize).Take(pageSize);

        return Ok(paginatedBooks);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound("Libro no encontrado.");

        book.Title = updatedBook.Title;
        book.Isbn = updatedBook.Isbn;
        book.PublicationYear = updatedBook.PublicationYear;

        _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync();
        return Ok(new { Mensaje = "Libro actualizado con éxito.", book });
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound("Libro no encontrado.");

        _bookRepository.Delete(book);
        await _bookRepository.SaveChangesAsync();
        return Ok(new { Mensaje = "Libro eliminado correctamente." });
    }
}