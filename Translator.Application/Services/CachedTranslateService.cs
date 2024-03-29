﻿using System;
using System.Threading.Tasks;
using Translator.Domain.Entities;
using Translator.Domain.Repositories;

namespace Translator.Application.Services
{
	public class CachedTranslateService : ICachedTranslateService
	{
		private readonly ITranslateService _translateSvc;
		private readonly ITranslationRepository _translationRepo;
		private readonly TranslationSettings _settings;

		public CachedTranslateService(ITranslateService translateSvc, ITranslationRepository translationRepo,
			TranslationSettings settings)
		{
			_translateSvc = translateSvc;
			_translationRepo = translationRepo;
			_settings = settings;
		}

		public async Task<string> Translate(string query, string fromLanguage, string toLanguage)
		{
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException(nameof(query));
			if (string.IsNullOrEmpty(fromLanguage))
				fromLanguage = _settings.DefaultLanguageInput ?? throw new ArgumentNullException(nameof(fromLanguage));
			if (string.IsNullOrEmpty(toLanguage))
				toLanguage = _settings.DefaultLanguageOutput ?? throw new ArgumentNullException(nameof(toLanguage));

			var translation = await _translationRepo.FindAsync(query, fromLanguage, toLanguage);
			if (translation?.To != null)
				return translation.To;

			var result = await _translateSvc.Translate(query, fromLanguage, toLanguage);

			if (result != null)
			{
				translation = new Translation
				{
					From = query,
					To = result.TranslatedText,
					FromLanguage = result.FromLanguage,
					ToLanguage = result.ToLanguage,
					TimeStamp = DateTime.UtcNow
				};
				await _translationRepo.AddAsync(translation);
				return result.TranslatedText;
			}

			return null;
		}
	}
}
