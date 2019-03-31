using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Translator.Application.Services;
using Translator.Dto;

namespace Translator.Controllers
{
	public class SearchController : ApiController
	{
		private readonly ICachedTranslateService _translateSvc;

		public SearchController(ICachedTranslateService translateSvc)
		{
			_translateSvc = translateSvc;
		}

		public async Task<HttpResponseMessage> Get(string query)
		{
			if (string.IsNullOrEmpty(query))
				return Request.CreateResponse(HttpStatusCode.OK, new TranslateResult(string.Empty));

			try
			{
				var result = await _translateSvc.Translate(query, null, null);
				return Request.CreateResponse(HttpStatusCode.OK, new TranslateResult(result));
			}
			catch (Exception exc)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exc.Message);
			}
		}
	}
}
