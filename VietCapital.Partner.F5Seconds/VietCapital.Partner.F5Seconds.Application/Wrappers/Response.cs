using System;
using System.Collections.Generic;
using System.Text;

namespace VietCapital.Partner.F5Seconds.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(bool succeed, T dt, string mess = null, List<string> error = null,int c = 200)
        {
            succeeded = succeed;
            message = mess;
            data = dt;
            errors = error;
            code = c;
        }
        public Response(T data, string message = null)
        {
            succeeded = true;
            this.message = message;
            this.data = data;
        }
        public Response(string message)
        {
            succeeded = false;
            this.message = message;
        }
        public bool succeeded { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public List<string> errors { get; set; }
        public T data { get; set; }
    }

    public class ResponseBase
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
