using BookingSystem.Application.DTOs.Hotel;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Mapping
{
    public static class HotelMapper
    {
        public static HotelResponseDto ToResponseDto(Hotel hotel)
        {
            return new HotelResponseDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                City = hotel.City,
                IsActive = hotel.IsActive
            };
        }

        public static HotelDetailedResponseDto ToDetailedResponseDto(Hotel hotel)
        {
            return new HotelDetailedResponseDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                IsActive = hotel.IsActive,
                CreatedAt = hotel.CreatedAt,
                UpdatedAt = hotel.UpdatedAt,
            };
        }

        public static Hotel ToEntity(HotelRequestDto request)
        {
            return new Hotel
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City,
            };
        }

        public static void UpdateEntity(HotelRequestDto request, Hotel hotel)
        {
            hotel.Name = request.Name;
            hotel.Address = request.Address;
            hotel.City = request.City;
        }
    }
}

