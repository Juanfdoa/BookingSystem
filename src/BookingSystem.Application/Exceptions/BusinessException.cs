using System.Net;

namespace BookingSystem.Application.Exceptions
{
    public sealed class BusinessException : ApiException
    {
        public BusinessException(string message) : base(message, HttpStatusCode.UnprocessableEntity)
        {
        }
    }
}
