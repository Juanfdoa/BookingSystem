using BookingSystem.Application.DTOs.Room;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Mapping
{
    public static class RoomMapper
    {
        public static RoomResponseDto ToResponseDto(Room room)
        {
            return new RoomResponseDto
            {
                Id = room.Id,
                Number = room.Number,
                Type = room.Type,
                IsActive = room.IsActive,
                Capacity = room.Capacity,
                BasePrice = room.BasePrice,
                
            };
        }

        public static RoomDetailedResponseDto ToDetailedResponseDto(Room room)
        {
            return new RoomDetailedResponseDto
            {
                Id = room.Id,
                HotelId = room.HotelId,
                Number = room.Number,
                Type = room.Type,
                Capacity = room.Capacity,
                BasePrice = room.BasePrice,
                Taxes = room.Taxes,
                Location = room.Location,
                IsActive = room.IsActive,
                CreatedAt = room.CreatedAt,
                UpdatedAt = room.UpdatedAt,
            };
        }

        public static Room ToEntity(RoomRequestDto request)
        {
            return new Room
            {
                HotelId = request.HotelId,
                Number = request.Number,
                Type = request.Type,
                Capacity = request.Capacity,
                BasePrice = request.BasePrice,
                Taxes = request.Taxes,
                Location = request.Location,
            };
        }

        public static void UpdateEntity(RoomUpdateRequestDto request, Room room)
        {
            room.Number = request.Number;
            room.Type = request.Type;
            room.Capacity = request.Capacity;
            room.BasePrice = request.BasePrice;
            room.Taxes = request.Taxes;
            room.Location = request.Location;
        }

        public static AvailableRoomResponseDto ToAvailableResponseDto(Room room)
        {
            return new AvailableRoomResponseDto
            {
                RoomId = room.Id,
                HotelId = room.HotelId,
                HotelName = room.Hotel.Name,
                City = room.Hotel.City,
                RoomNumber = room.Number,
                Type = room.Type,
                Capacity = room.Capacity,
                BasePrice = room.BasePrice,
                Taxes = room.Taxes,
                TotalPrice = room.BasePrice + room.Taxes
            };
        }
    }
}
