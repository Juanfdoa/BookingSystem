using BookingSystem.Application.DTOs.Reservation;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.Interfaces.Services
{
    public interface IReservationService
    {
        Task<ApiResponse<IEnumerable<ReservationResponseDto>>> GetAllAsync(ReservationFilter filter, Pagination pagination);
        Task<ApiResponse<ReservationDetailedResponseDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<ReservationDetailedResponseDto>> CreateAsync(ReservationRequestDto request);
        Task UpdateReservationStatus(Guid id, ReservationStatus status);
    }
}
