using System.Net;
using HRM.Models.Cores;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Models.Responses.Bases
{
    public class OkResponse : JsonResult 
    {
        public OkResponse(object value) : base(value)
        {
            StatusCode = (int) HttpStatusCode.OK;
        }

        public OkResponse(object value, object serializerSettings) : base(value, serializerSettings)
        {
            StatusCode = (int) HttpStatusCode.OK;
        }
    }
}