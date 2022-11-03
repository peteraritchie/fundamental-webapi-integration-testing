using Microsoft.OpenApi.Models; // Microsoft.OpenApi.Readers 1.4.1
using Microsoft.OpenApi.Readers; // Microsoft.OpenApi.Readers 1.4.1

namespace TestProject1;

public class WebApiShouldBase : IDisposable
{
	private readonly string swaggerUriText;
	protected readonly HttpClient WebApiClient;

	protected WebApiShouldBase() : this("/swagger/v1/swagger.json") { }
	protected OpenApiDocument? OpenApiDocument { get; private set; }
	protected WebApiShouldBase(string swaggerUriText)
	{
		this.swaggerUriText = swaggerUriText;
		var factory = new MyApplicationFactory();
		factory.WithWebHostBuilder(builder =>
		{
		});
		WebApiClient = factory.CreateClient();
		OpenApiDocument = factory.OpenApiDocument;
	}

	protected virtual void Dispose(bool isDisposing)
	{
		if (isDisposing)
		{
			Dispose();
		}
	}

	public void Dispose()
	{
		WebApiClient.Dispose();
	}

	protected async Task<OpenApiDocument> GetOpenApiDocumentAsync()
	{
		return (await new OpenApiStreamReader()
				.ReadAsync(await GetOpenApiDocumentStreamAsync().ConfigureAwait(false)).ConfigureAwait(false))
			.OpenApiDocument;
	}

	protected Task<Stream> GetOpenApiDocumentStreamAsync()
	{
		return WebApiClient.GetStreamAsync(swaggerUriText);
	}
}
