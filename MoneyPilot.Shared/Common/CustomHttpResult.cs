using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MoneyPilot.Shared.Common
{
    public static class CustomHttpResult
    {
        public static IActionResult GetResponse<T>(this CustomResponse<T> customResponse)
        {
            var errorDetail = new
            {
                Errors = customResponse.Errors,
                ErrorMessage = customResponse.ErrorMessage,
                StatusCode = (int)customResponse.ResponseCode,
            };

            return new ObjectResult(customResponse.ResponseCode == System.Net.HttpStatusCode.OK ? customResponse.Data : errorDetail)
            {
                StatusCode = (int)customResponse.ResponseCode
            };
        }

        public static CustomResponse<T> NotFound<T>(string errorMessage)
        {
            var customResponse = new CustomResponse<T>
            {
                ErrorMessage = errorMessage,
                ResponseCode = System.Net.HttpStatusCode.NotFound,
            };
            return customResponse;
        }


        public static CustomResponse<T> BadRequest<T>(string errorMessage, Dictionary<string, List<string>>? errors = null)
        {
            var customResponse = new CustomResponse<T>
            {
                ErrorMessage = errorMessage,
                ResponseCode = System.Net.HttpStatusCode.BadRequest,
                Errors = errors,
            };
            return customResponse;
        }

        public static CustomResponse<T> Ok<T>(T result)
        {
            var customResponse = new CustomResponse<T>
            {
                ResponseCode = System.Net.HttpStatusCode.OK,
                Data = result,
            };
            return customResponse;
        }

        public static CustomResponse<T> TooManyRequests<T>(string errorMessage)
        {
            var customResponse = new CustomResponse<T>
            {
                ResponseCode = System.Net.HttpStatusCode.TooManyRequests,
                ErrorMessage = errorMessage,
            };
            return customResponse;
        }

        public static CustomResponse<T> UnAuthorized<T>(string errorMessage)
        {
            var customResponse = new CustomResponse<T>
            {
                ErrorMessage = errorMessage,
                ResponseCode = System.Net.HttpStatusCode.Unauthorized,
            };
            return customResponse;
        }
    }
}
