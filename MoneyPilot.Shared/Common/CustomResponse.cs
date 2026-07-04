using System.Net;

namespace MoneyPilot.Shared.Common
{
    public class CustomResponse<T>
    {
        public HttpStatusCode ResponseCode { get; set; }
        public string ErrorMessage { get; set; }

        public Dictionary<string, List<string>>? Errors { get; set; }
        public T Data { get; set; }
    }
}
