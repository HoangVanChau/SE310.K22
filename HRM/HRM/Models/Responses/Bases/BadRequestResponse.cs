using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Models.Responses.Bases
{
    public class BadRequestResponse : JsonResult
    {
        public BadRequestResponse(object value) : base(value)
        {
            StatusCode = (int) HttpStatusCode.BadRequest;
        }

        public BadRequestResponse(object value, object serializerSettings) : base(value, serializerSettings)
        {
            StatusCode = (int) HttpStatusCode.BadRequest;
        }
    }
}