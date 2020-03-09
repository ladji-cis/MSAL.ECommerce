using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MSAL.ECommerce.ClientDesk.Exceptions
{
    public class ApiCallException : Exception
    {
        public ApiCallException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
