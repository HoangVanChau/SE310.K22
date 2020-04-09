using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Models.Responses.Bases
{
    public class NotAllowResponse : JsonResult
    {
        public NotAllowResponse(object value) : base(value)
        {
            StatusCode = (int) HttpStatusCode.MethodNotAllowed;
        }

        public NotAllowResponse(object value, object serializerSettings) : base(value, serializerSettings)
        {
            StatusCode = (int) HttpStatusCode.MethodNotAllowed;
        }
    }
}