using Google.Cloud.Translation.V2;
using System.Threading.Tasks;
using Translator.Application.Dto;
using Translator.Application.Services;

namespace Translator.Infrastructure.Services
{
	public class GoogleTranslateService : ITranslateService
	{
		private readonly TranslationClient _client;

		public GoogleTranslateService(TranslationClient client)
		{
			_client = client;
		}

		public async Task<TranslateResult> Translate(string query, string fromLanguage, string toLanguage)
		{
			var response = await _client.TranslateTextAsync(
				text: query,
				targetLanguage: toLanguage,
				sourceLanguage: fromLanguage);
			return new TranslateResult(response.TranslatedText, 
				response.DetectedSourceLanguage ?? response.SpecifiedSourceLanguage, 
				response.TargetLanguage);
		}
	}
}
