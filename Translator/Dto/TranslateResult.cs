using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Translator.Dto
{
	public class TranslateResult
	{
		public string TranslatedText { get; set; }

		public TranslateResult(string translatedText)
		{
			TranslatedText = translatedText;
		}
	}
}