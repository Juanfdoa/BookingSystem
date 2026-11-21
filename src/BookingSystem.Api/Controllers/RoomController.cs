using BookingSystem.Application.DTOs.Room;
using BookingSystem.Application.Interfaces.Services;
using BookingSystem.Application.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Gets all rooms for a specific hotel.
        /// </summary>
        [HttpGet("/api/hotels/{hotelId:guid}/rooms")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoomResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByHotel(Guid hotelId, [FromQuery] RoomFilter filter)
        {
            var response = await _roomService.GetAllAsync(hotelId, filter);
            return Ok(response);
        }

        /// <summary>
        /// Get room by id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<RoomDetailedResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _roomService.GetByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Create a room
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(RoomDetailedResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RoomRequestDto request)
        {
            var room = await _roomService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = room.Data!.Id }, room);
        }

        /// <summary>
        /// Update a room
        /// </summary>
        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoomUpdateRequestDto request)
        {
            await _roomService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Delete a room
        /// </summary>
        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roomService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// availability search for rooms based on the provided filter criteria.
        /// </summary>
        [HttpGet("availability")]
        public async Task<IActionResult> SearchAvailability([FromQuery] AvailabilityFilter filter, [FromQuery] Pagination pagination, CancellationToken cancellationToken)
        {
            var response = await _roomService.SearchAvailableAsync(filter, pagination, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Update the room status (active/inactive)
        /// </summary>
        [Authorize]
        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateStatus(Guid id, bool isActive)
        {
            await _roomService.UpdateStatus(id, isActive);
            return NoContent();
        }
    }
}