using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Translator.Models
{
	public class SearchResultViewModel
	{
		public string TranslatedText { get; set; }

		public SearchResultViewModel(string translatedText)
		{
			TranslatedText = translatedText;
		}
	}
}