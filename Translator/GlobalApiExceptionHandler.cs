using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace Translator
{
	public class GlobalApiExceptionHandler : IExceptionHandler
	{
		public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}