using Newtonsoft.Json;
using System.Text;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Exceptions;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance;
using TestTask_ManageTreeAndJournalOfExceptions.Web.Models;

namespace TestTask_ManageTreeAndJournalOfExceptions.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IJournalRepository journalRepository)
        {
            var requestBody = await ReadRequestBody(context);

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var timestamp = DateTime.UtcNow;

                await SaveExceptionDetailsAsync(context, requestBody, ex, timestamp, journalRepository);

                await HandleExceptionAsync(context, ex, timestamp);
            }
        }

        private async Task<string> ReadRequestBody(HttpContext context)
        {
            context.Request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);

            context.Request.Body.Position = 0;

            return requestBody;
        }

        private async Task SaveExceptionDetailsAsync(
            HttpContext context, string requestBody, Exception ex, DateTime timestamp, IJournalRepository journalRepository)
        {
            var requestDetails = new
            {
                body = requestBody,
                queryParameters = context.Request.Query,
                stackTrace = ex.StackTrace,
            };

            await journalRepository.AddAsync(new Journal
            {
                EventId = timestamp.Ticks,
                CreatedAt = timestamp,
                Text = JsonConvert.SerializeObject(requestDetails)
            });
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, DateTime timestamp)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                Type = "Exception",
                Id = timestamp.Ticks,
                Data = new ErrorDetails { Message = $"Internal server error ID = {timestamp.Ticks}" }
            };

            if (ex is SecureException)
            {
                errorResponse.Type = ex.GetType().Name.Replace("Exception", string.Empty);
                errorResponse.Data.Message = ex.Message;
            }

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
