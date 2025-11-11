using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Middleware
{
	public static class ExceptionHandlingMiddlewareExtensions
	{
		public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandlingMiddleware>();
		}
	}
}
