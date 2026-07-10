using BookingSystem.Application.DTOs.Reservation;
using BookingSystem.Application.Interfaces.Services;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// Get all reservations
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ReservationResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] ReservationFilter filter,[FromQuery] Pagination pagination)
        {
            var response = await _reservationService.GetAllAsync(filter, pagination);
            return Ok(response);
        }

        /// <summary>
        /// Get reservation by id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<ReservationDetailedResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _reservationService.GetByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Create reservation
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationDetailedResponseDto),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ReservationRequestDto request)
        {
            var reservation = await _reservationService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Data!.Id }, reservation);
        }

        /// <summary>
        /// Update reservation status
        /// </summary>
        [Authorize]
        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] ReservationStatus status)
        {
            await _reservationService.UpdateReservationStatus(id, status);
            return NoContent();
        }
    }
}
