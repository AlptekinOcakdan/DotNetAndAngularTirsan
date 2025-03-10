﻿using Core.Utilities.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Core.Extensions {
    public class ExceptionMiddleware {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext) {
            try {
                await _next(httpContext);
            }
            catch (Exception e) {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e) {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";
            IEnumerable<ValidationFailure> errors;
            if (e.GetType() == typeof(ValidationException)) {
                message = "Validation Error";
                errors = ((ValidationException)e).Errors;
                httpContext.Response.StatusCode = 422;
                return httpContext.Response.WriteAsync(new ValidationErrorDetails {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = message,
                    Errors = errors
                }.ToString());
            }

            else if (e.GetType() == typeof(AuthorizationException)) {
                message = e.Message;
                httpContext.Response.StatusCode = 401;
                return httpContext.Response.WriteAsync(new ErrorDetails {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = message
                }.ToString());
            }

            

            return httpContext.Response.WriteAsync(new ErrorDetails {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
  }
