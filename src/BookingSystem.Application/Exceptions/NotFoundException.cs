using System.Net;

namespace BookingSystem.Application.Exceptions
{
    public sealed class NotFoundException : ApiException
    {
        public NotFoundException(string message): base(message, HttpStatusCode.NotFound)
        {
        }
    }
}
