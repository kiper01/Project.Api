using Newtonsoft.Json;

namespace Project.Core.Models.Response
{
    public class ResultRequest
    {
        [JsonProperty(PropertyName = "code", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResultCode Status { get; }

        [JsonProperty(PropertyName = "message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; }

        [JsonProperty(PropertyName = "error", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ErrorInfo ErrorInfo { get; protected set; }
        public ResultRequest(ResultCode status, string message)
        {
            Status = status;
            Message = message;
        }

        public static ResultRequest Error(string message, string errorDetails)
        {
            var errorResult = new ResultRequest(ResultCode.Error, message)
            {
                ErrorInfo = new ErrorInfo(errorDetails)
            };
            return errorResult;
        }

        public static ResultRequest Ok(string message = "")
        {
            return new ResultRequest(ResultCode.Ok, message);
        }
    }
    public class ResultRequest<T> : ResultRequest
        where T : class
    {
        public ResultRequest(ResultCode status, string message, T dataObject)
            : base(status, message)
        {
            Data = dataObject;
        }

        [JsonProperty(PropertyName = "data", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public T Data { get; set; }

        #region Result Factory Methods

        public new static ResultRequest<T> Error(string message, string errorDetails)
        {
            var errorResult = new ResultRequest<T>(ResultCode.Error, message, null)
            {
                ErrorInfo = new ErrorInfo(errorDetails)
            };
            return errorResult;
        }

        public static ResultRequest<T> Ok(string message, T data)
        {
            return new ResultRequest<T>(ResultCode.Ok, message, data);
        }

        public static ResultRequest<T> Ok(T data)
        {
            return Ok(string.Empty, data);
        }

        #endregion
    }

    public enum ResultCode
    {
        Ok = 0,
        Error = 1
    }

    public class ErrorInfo
    {
        public ErrorInfo(string details)
        {
            Details = details;
        }
        [JsonProperty(PropertyName = "details", DefaultValueHandling = DefaultValueHandling.Include)]
        public string Details { get; }
    }
}
