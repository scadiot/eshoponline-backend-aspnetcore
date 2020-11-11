using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eshoponline.Infrastructure.Error
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public object Errors { get; set; }

        public HttpStatusCode Code { get; }
    }
}
