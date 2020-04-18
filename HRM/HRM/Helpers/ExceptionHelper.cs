using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using HRM.Models.Bases;
using HRM.Models.Cores;
using HRM.Models.Responses.Bases;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRM.Helpers
{
    public static class ExceptionHelper
    {
        public static JsonResult HandleError(Exception e)
        {
            return e switch
            {
                MongoWriteException exception => HandleMongoWriteException(exception),
                _ => new BadRequestResponse(new ErrorData
                {
                    Message = e.Message
                })
            };
        }

        private static JsonResult HandleMongoWriteException(MongoWriteException e)
        {
            return e.WriteError.Category switch
            {
                ServerErrorCategory.DuplicateKey => new BadRequestResponse(new ErrorData
                {
                    Message = "Trùng lặp trường",
                    Data = GetDuplicateField(e.WriteError.Message)
                }),
                _ => new BadRequestResponse(new ErrorData
                {
                    Message = e.WriteError.Message
                })
            };
        }

        private static Hashtable GetDuplicateField(string errorMessage)
        {
            var fieldName = Regex.Match(errorMessage, @"index: (.+?)_1")
                .Groups[1]
                .Value;
            var fieldValue = Regex.Match(errorMessage, @"{ : (.+?) }")
                .Groups[1]
                .Value
                .Replace('"'.ToString(), "");
            
            return new Hashtable {{"DuplicateField", fieldName}, {"DuplicateValue", fieldValue}};
        }
    }
}