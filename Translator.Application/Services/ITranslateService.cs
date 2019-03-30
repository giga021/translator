using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Application.Services
{
	public interface ITranslateService
	{
		Task<string> Translate(string query, string fromLanguage, string toLanguage);
	}
}
