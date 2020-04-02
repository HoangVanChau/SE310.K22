using System;
using System.Net;

namespace HRM.Models.Cores
{
    public class BaseResponse<T>
    {
        public String Message { get; set; }
        public HttpStatusCode Code { get; set; }
        public String Error { get; set; }
        public T Data { get; set; }
    }
}