using TimeZoneManager.Exceptions;
using TimeZoneManager.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Net.Http;
using TimeZoneManager.Authorization;

namespace TimeZoneManager.Api.Authentication
{
    /// <summary>
    /// Reads JWT token from HTTP Authorization header and stores permissions into ISession object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal sealed class JwtTokenRequiredAttribute : Attribute, IResourceFilter
    {
        private static readonly ExceptionToHttpCodeConverter _errorCodeConverter = new ExceptionToHttpCodeConverter();

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            try
            {
                ProcessAuthorization(context);
            }
            catch (Exception e) when ((e is UnauthorizedAccessException) || (e is InvalidTokenException))
            {
                ProcessError(context, e);
            }
            catch (Exception e)
            {
                ProcessError(context, new InvalidTokenException(e.Message, e));
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //nothing is required when returning from execution
        }

        private void ProcessAuthorization(ResourceExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue(nameof(HttpClient.DefaultRequestHeaders.Authorization),
                out StringValues authHeader))
            {
                FillSession(context, authHeader);
            }
            else
            {
                throw new UnauthorizedAccessException("Authorization header is missing");
            }
        }

        private static void FillSession(ResourceExecutingContext context, StringValues authHeader)
        {
            IJwtManager jwtManager = GetService<IJwtManager>(context);
            Session session = (Session)GetService<ISession>(context);

            JwtTokenPayload payload = jwtManager.GetPayload(authHeader);
            CheckUserInformation(payload.Username);

            session.Username = payload.Username;
            session.Permissions = payload.Permissions;
            session.Token = jwtManager.GetTokenFromAuthorizationHeader(authHeader);
        }

        private static void ProcessError(ResourceExecutingContext context, Exception e)
        {
            var logger = GetService<ILogger<JwtTokenRequiredAttribute>>(context);
            logger.LogError(e.Message);

            var errorCode = _errorCodeConverter.GetMessageAndHttpCode(e);
            context.HttpContext.Response.StatusCode = (int)errorCode.Status;
            context.Result = new JsonResult(errorCode.Message);
        }

        private static void CheckUserInformation(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidTokenException("Invalid authorization header");
            }
        }

        private static T GetService<T>(ActionContext context)
        {
            return context.HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}
