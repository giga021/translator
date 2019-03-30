using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Translator.Controllers
{
	public class SearchController : ApiController
	{
		public async Task<HttpResponseMessage> Get(string query)
		{
			return await Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, new
			{
				result = DateTime.Now.Ticks.ToString()
			}));
		}
	}
}
