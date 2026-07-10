using BookingSystem.Application.DTOs.Auth;

namespace BookingSystem.Application.Interfaces.Providers
{
    public interface IJwtService
    {
        JwtTokenResponseDto GenerateToken(JwtUserResponseDto user,IList<string> roles);
    }
}
