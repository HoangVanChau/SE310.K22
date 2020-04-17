using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Models.Responses.Bases
{
    public class NotFoundResponse : JsonResult
    {
        public NotFoundResponse(object value) : base(value)
        {
            StatusCode = (int) HttpStatusCode.NotFound;
        }

        public NotFoundResponse(object value, object serializerSettings) : base(value, serializerSettings)
        {
            StatusCode = (int) HttpStatusCode.NotFound;
        }
    }
}