using Moq;
using System;
using System.Threading.Tasks;
using Translator.Application;
using Translator.Application.Dto;
using Translator.Application.Services;
using Translator.Domain.Entities;
using Translator.Domain.Repositories;
using Xunit;

namespace Translator.Tests.Application.Services
{
	public class CachedTranslateServiceTests
	{
		[Fact]
		public async Task Translate_FromLanguageNull_UsesDefault()
		{
			var fakeTranslateSvc = new Mock<ITranslateService>();
			var fakeRepo = new Mock<ITranslationRepository>();
			var settings = new TranslationSettings("AA", "BB");
			var cached = new CachedTranslateService(fakeTranslateSvc.Object, fakeRepo.Object, settings);

			var translation = await cached.Translate("moon", null, "FR");

			fakeRepo.Verify(x => x.FindAsync("moon", "AA", "FR"), Times.Once());
		}

		[Fact]
		public async Task Translate_ToLanguageNull_UsesDefault()
		{
			var fakeTranslateSvc = new Mock<ITranslateService>();
			var fakeRepo = new Mock<ITranslationRepository>();
			var settings = new TranslationSettings("AA", "BB");
			var cached = new CachedTranslateService(fakeTranslateSvc.Object, fakeRepo.Object, settings);

			var translation = await cached.Translate("moon", "EN", null);

			fakeRepo.Verify(x => x.FindAsync("moon", "EN", "BB"), Times.Once());
		}

		[Fact]
		public async Task Translate_AlreadyInCache_DoesntInvokeExternalService()
		{
			var fakeTranslateSvc = new Mock<ITranslateService>();
			var fakeRepo = new Mock<ITranslationRepository>();
			fakeRepo.Setup(x => x.FindAsync("moon", "EN", "FR")).ReturnsAsync(new Translation { From = "moon", To = "lune" });
			var settings = new TranslationSettings("AA", "BB");
			var cached = new CachedTranslateService(fakeTranslateSvc.Object, fakeRepo.Object, settings);

			var translation = await cached.Translate("moon", "EN", "FR");

			Assert.Equal("lune", translation);
			fakeTranslateSvc.Verify(x => x.Translate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
		}

		[Fact]
		public async Task Translate_NotInCache_InvokesExternalServiceAndAddsToCache()
		{
			var fakeTranslateSvc = new Mock<ITranslateService>();
			fakeTranslateSvc.Setup(x => x.Translate("moon", "EN", "FR")).ReturnsAsync(new TranslateResult("lune", "EN", "FR"));
			var fakeRepo = new Mock<ITranslationRepository>();
			var settings = new TranslationSettings("AA", "BB");
			var cached = new CachedTranslateService(fakeTranslateSvc.Object, fakeRepo.Object, settings);

			var translation = await cached.Translate("moon", "EN", "FR");

			Assert.Equal("lune", translation);
			fakeRepo.Verify(x => x.AddAsync(It.Is<Translation>(t => t.From == "moon" && t.To == "lune" && t.FromLanguage == "EN" && t.ToLanguage == "FR")), Times.Once());
		}
	}
}
