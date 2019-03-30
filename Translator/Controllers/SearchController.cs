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
		private readonly ITranslateService _translateSvc;

		public SearchController(ITranslateService translateSvc)
		{
			_translateSvc = translateSvc;
		}

		public async Task<HttpResponseMessage> Get(string query)
		{
			var result = await _translateSvc.Translate(query, null, null);
			return Request.CreateResponse(HttpStatusCode.OK, new TranslateResult(result));
		}
	}
}
