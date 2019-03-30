using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Application.Dto;

namespace Translator.Application.Services
{
	public interface ITranslateService
	{
		Task<TranslateResult> Translate(string query, string fromLanguage, string toLanguage);
	}
}
