using BookingSystem.Application.DTOs.Hotel;
using BookingSystem.Application.Interfaces.Services;
using BookingSystem.Application.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Get all hotels
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] HotelFilter filter, [FromQuery] Pagination pagination)
        {
            var response = await _hotelService.GetAllAsync(filter, pagination);
            return Ok(response);
        }

        /// <summary>
        /// Get hotel by id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<HotelDetailedResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _hotelService.GetByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Create a hotel
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(HotelDetailedResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] HotelRequestDto request)
        {
            var hotel = await _hotelService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = hotel.Data!.Id },hotel);
        }

        /// <summary>
        /// Update a hotel
        /// </summary>
        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] HotelRequestDto request)
        {
            await _hotelService.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Delete a hotel
        /// </summary>
        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _hotelService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Update the hotel status (active/inactive)
        /// </summary>
        [Authorize]
        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateStatus(Guid id, bool isActive)
        {
            await _hotelService.UpdateStatus(id, isActive);
            return NoContent();
        }
    }
}
