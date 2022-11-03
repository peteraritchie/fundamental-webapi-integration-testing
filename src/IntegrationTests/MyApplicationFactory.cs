using Microsoft.AspNetCore.Mvc.Testing; // Microsoft.AspNetCore.Mvc.Testing (6.0.10)
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TestProject1
{
	public class MyApplicationFactory : WebApplicationFactory<Program>
	{
		public OpenApiDocument? OpenApiDocument { get; private set; }

		protected override IHost CreateHost(IHostBuilder builder)
		{
			var host = base.CreateHost(builder);
			using var scope = host.Services.CreateScope();
			var sp = scope.ServiceProvider;
			var swaggerProvider = sp.GetRequiredService<ISwaggerProvider>();
			//var x = swaggerProvider as SwaggerGenerator;
			var swaggerGeneratorOptions = sp.GetRequiredService<IOptions<SwaggerGeneratorOptions>>().Value;
			//var swaggerOptions = sp.GetRequiredService<IOptions<SwaggerOptions>>().Value;
			//var swaggerUiOptions
			//	= sp.GetService<SwaggerUIOptions>() ?? new SwaggerUIOptions();
			OpenApiDocument = swaggerProvider.GetSwagger(swaggerGeneratorOptions.SwaggerDocs.First().Key);

			return host;
		}
	}
}