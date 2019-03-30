using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Domain.Entities;

namespace Translator.Domain.Repositories
{
	public interface ITranslationRepository
	{
		Task<Translation> FindAsync(string query, string fromLanguage, string toLanguage);
		Task AddAsync(Translation translation);
	}
}
