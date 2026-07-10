namespace BookingSystem.Application.Interfaces.Providers
{
    public interface ITokenEncoder
    {
        string Encode(string value);
        string Decode(string value);
    }
}
