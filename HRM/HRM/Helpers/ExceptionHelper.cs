using System;
using System.Net;
using HRM.Models.Responses.Bases;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRM.Helpers
{
    public static class ExceptionHelper
    {
        public static JsonResult HandleError(Exception e)
        {
            switch (e)
            {
                case MongoConnectionException _:
                    return new JsonResult(new {StatusCode = (int) HttpStatusCode.InternalServerError});
                case MongoWriteException _:
                    return new JsonResult(new {StatusCode = (int) HttpStatusCode.BadRequest});
                case MongoBulkWriteException _:
                    return new JsonResult(new {StatusCode = (int) HttpStatusCode.BadRequest});
                default:
                    return new BadRequestResponse(null);
            }
        }
    }
}