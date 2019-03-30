using System.Threading.Tasks;

namespace Translator.Application.Services
{
	public interface ICachedTranslateService
	{
		Task<string> Translate(string query, string fromLanguage, string toLanguage);
	}
}
