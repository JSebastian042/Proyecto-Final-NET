using Library.Application.Interfaces;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IRepository<Reservation> _reservationRepository;

    public ReservationsController(IRepository<Reservation> reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(int bookId, int userId)
    {
        var reservation = new Reservation
        {
            BookId = bookId,
            UserId = userId,
            ReservationDate = DateTime.UtcNow,
            Status = "Activa"
        };

        await _reservationRepository.AddAsync(reservation);
        await _reservationRepository.SaveChangesAsync();
        return Ok(new { Mensaje = "Reserva creada con éxito.", reservation });
    }
}