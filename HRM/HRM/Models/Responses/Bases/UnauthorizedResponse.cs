using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Models.Responses.Bases
{
    public class UnauthorizedResponse: JsonResult
    {
        public UnauthorizedResponse(object value) : base(value)
        {
            StatusCode = (int) HttpStatusCode.Unauthorized;
        }

        public UnauthorizedResponse(object value, object serializerSettings) : base(value, serializerSettings)
        {
            StatusCode = (int) HttpStatusCode.Unauthorized;
        }
    }
}