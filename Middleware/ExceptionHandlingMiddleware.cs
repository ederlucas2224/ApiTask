using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionHandlingMiddleware(
			RequestDelegate next,
			ILogger<ExceptionHandlingMiddleware> logger,
			IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}
		/// <summary>
		/// Invocacion Asyncrona
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error no manejado: {Message}", ex.Message);
				await HandleExceptionAsync(context, ex);
			}
		}
		/// <summary>
		/// Metodo para excepcion
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exception"></param>
		/// <returns></returns>
		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			var response = new
			{
				error = exception.Message,
				details = _env.IsDevelopment() ? exception.StackTrace : null
			};

			context.Response.StatusCode = exception switch
			{
				NotFoundException => (int)HttpStatusCode.NotFound,
				BusinessException => (int)HttpStatusCode.BadRequest,
				ValidationException => (int)HttpStatusCode.BadRequest,
				_ => (int)HttpStatusCode.InternalServerError
			};

			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
			var json = JsonSerializer.Serialize(response, options);

			await context.Response.WriteAsync(json);
		}
	}

	/// <summary>
	/// Valida excepcion
	/// </summary>
	public class ValidationException : Exception
	{
		public ValidationException(string message) : base(message) { }
	}
}
