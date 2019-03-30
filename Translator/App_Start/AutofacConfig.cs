using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Google.Cloud.Translation.V2;
using System.Web.Http;
using System.Web.Mvc;
using Translator.Application;
using Translator.Application.Services;
using Translator.Infrastructure.Services;
using Translator.Properties;

namespace Translator.App_Start
{
	public class AutofacConfig
	{
		private static void AddApplicationServices(ContainerBuilder builder)
		{
			builder.RegisterInstance(TranslationClient.CreateFromApiKey(Settings.Default.GoogleTranslateKey));
			builder.RegisterInstance(new TranslationSettings(Settings.Default.DefaultLanguageOutput));
			builder.RegisterType<GoogleTranslateService>().As<ITranslateService>();
		}

		private static void ConfigureMvc()
		{
			var builder = new ContainerBuilder();

			builder.RegisterControllers(typeof(MvcApplication).Assembly);
			AddApplicationServices(builder);

			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}

		private static void ConfigureWebApi()
		{
			var builder = new ContainerBuilder();

			builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
			AddApplicationServices(builder);

			var container = builder.Build();
			GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}

		public static void Configure()
		{
			ConfigureMvc();
			ConfigureWebApi();
		}
	}
}