using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BoardCommander.Exceptions
{
    public abstract class BaseApiException : Exception
    {
        public int StatusCode { get; set; }
        public BaseApiException(int statusCode, string message= "") : base(message)
        {
            StatusCode = statusCode;
        }

    }
}
