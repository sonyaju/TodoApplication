using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;

namespace ToDoApp.Models
{
    /// <summary>
    /// Generic response class that can be used for any controller for the project
    /// </summary>
    public class ResponseHeader
    {
        public string statusMessage { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public List<Error> error { get; set; }
    }

    /// <summary>
    /// Generic errolist response class
    /// </summary>
    public class Error
    {
        public string errorMessage { get; set; }
        public string errorCode { get; set; }

        public Error(string errorMessage, string errorCode)
        {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }

    }


}