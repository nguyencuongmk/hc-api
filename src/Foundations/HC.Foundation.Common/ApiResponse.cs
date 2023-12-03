using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace HC.Foundation.Common
{
    public class ApiResponse
    {
        public static readonly ApiResponse GeneralError = new()
        {
            Result = new ResponseResult
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest)
            }
        };

        public ApiResponse()
        {
            ModelErrors = new List<ModelError>();
        }

        public ApiResponse(dynamic success)
        {
            Data = success;
        }

        public ApiResponse(int errorCode, string message)
        {
            Result = new ResponseResult
            {
                StatusCode = errorCode,
                Message = message
            };
        }

        public ApiResponse(int errorCode)
        {
            Result = new ResponseResult
            {
                StatusCode = errorCode,
                Message = ReasonPhrases.GetReasonPhrase(errorCode)
            };
        }

        [JsonProperty("data")]
        public dynamic Data { get; set; }

        [JsonProperty("heading")]
        public ResponseHeading Heading { get; set; } = new ResponseHeading
        {
            Timestamp = DateTime.Now.TimeOfDay.ToString()
        };

        [JsonProperty("result")]
        public ResponseResult Result { get; set; } = new ResponseResult
        {
            StatusCode = StatusCodes.Status200OK,
            Message = ReasonPhrases.GetReasonPhrase(StatusCodes.Status200OK),
            Description = string.Empty
        };

        [JsonProperty("model_errors")]
        public List<ModelError> ModelErrors { get; set; }

        public static ApiResponse GetResponseResult(ApiResponse response, int statusCode, string description = "")
        {
            response.Result.StatusCode = statusCode;
            response.Result.Message = ReasonPhrases.GetReasonPhrase(statusCode);
            response.Result.Description = description;
            return response;
        }
    }

    public class ResponseHeading
    {
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }

    public class ResponseResult
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
    }

    [Serializable]
    public class ModelError
    {
        [JsonProperty("code")]
        public string ErrorCode { get; set; }

        [JsonProperty("field")]
        public string ErrorField { get; set; }

        [JsonProperty("message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}