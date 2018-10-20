using Newtonsoft.Json;

namespace DotNetApisForAngularProjects.Helpers
{
    public class ApiErrorHelper
    {
        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; private set; }

        public ApiErrorHelper(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
        }

        public ApiErrorHelper(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
        }

        /**
        status codes:
        Ok() for 200 OK;
        BadRequest(object error) for 400 Bad Request;
        NotFound(object value) for 404 Not Found;
        Forbid() for 403 Forbidden;
        Unauthorized() for 401 Unauthorized;
        StatusCode(int statusCode, object value) for choosing a custom status code.
         */
    }
}