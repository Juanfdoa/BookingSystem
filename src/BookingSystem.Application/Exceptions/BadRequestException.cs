using System.Net;

namespace BookingSystem.Application.Exceptions
{
    public sealed class BadRequestException : ApiException
    {
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
