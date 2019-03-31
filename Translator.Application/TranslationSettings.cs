using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Application
{
	public class TranslationSettings
	{
		public string DefaultLanguageInput { get; }
		public string DefaultLanguageOutput { get; }

		public TranslationSettings(string defaultLanguageInput, string defaultLanguageOutput)
		{
			DefaultLanguageInput = defaultLanguageInput;
			DefaultLanguageOutput = defaultLanguageOutput;
		}
	}
}
