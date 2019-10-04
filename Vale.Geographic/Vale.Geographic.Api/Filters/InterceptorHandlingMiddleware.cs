using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Vale.Geographic.Api.Filters
{
    public class InterceptorHandlingMiddleware
    {
        private readonly string _languageHeader = "Accept-Language";
        private readonly RequestDelegate _next;

        public InterceptorHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                var headers = context.Request.Headers;
                ;

                if (headers[_languageHeader].Count > 0)
                {
                    var language = headers[_languageHeader].First();
                    var culture = CultureInfo.CreateSpecificCulture(language);

                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var mensages = new List<string>();

            //if (exception is UnauthorizedAccessException) code = HttpStatusCode.NotFound;
            if (exception is NotImplementedException)
            {
                code = HttpStatusCode.NotImplemented;
            }
            else if (exception is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
            }
            else if (exception is ValidationException validationException)
            {
                code = HttpStatusCode.BadRequest;
                foreach (var item in validationException.Errors) mensages.Add(item.ErrorMessage);
            }

            ;

            if (!mensages.Any())
                mensages.Add(exception.Message);

            var result = JsonConvert.SerializeObject(new Error { status = (int)code, messages = mensages });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}