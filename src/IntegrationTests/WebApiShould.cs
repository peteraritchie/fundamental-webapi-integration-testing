using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers; // Microsoft.OpenApi.Readers 1.4.1

namespace TestProject1
{
	public sealed class WebApiShould : WebApiShouldBase
	{
		[Fact]
		public async Task ProduceValidOpenApi()
		{
			var readerResult = await new OpenApiStreamReader()
				.ReadAsync(await GetOpenApiDocumentStreamAsync().ConfigureAwait(false)).ConfigureAwait(false);
			Assert.NotNull(OpenApiDocument);
			Assert.Empty(readerResult.OpenApiDiagnostic.Errors);
			Assert.NotEmpty(readerResult.OpenApiDocument.Paths);
		}

		[Fact]
		public async Task EndpointsRespondWithCorrectMediaTypeToGet()
		{
			Assert.NotNull(OpenApiDocument);
			var pathsWithGetOperations = OpenApiDocument.Paths.Where(w => w.Value.Operations.ContainsKey(OperationType.Get));

			foreach (var path in pathsWithGetOperations)
			{
				var getOperation = path.Value.Operations[OperationType.Get];
				var response = await WebApiClient.GetAsync(path.Key).ConfigureAwait(false);
				Assert.True(response.Content.Headers.ContentType?.MediaType == getOperation.Responses["200"].Content.First().Key);
			}
		}

		[Fact]
		public async Task EndpointsRespondOkToGet()
		{
			var pathsWithGetOperations = OpenApiDocument.Paths.Where(w => w.Value.Operations.ContainsKey(OperationType.Get));

			foreach (var e in pathsWithGetOperations)
			{
				var response = await WebApiClient.GetAsync(e.Key).ConfigureAwait(false);
				Assert.True(response.IsSuccessStatusCode);
			}
		}
#if false
        [Fact]
        public async Task ShouldGetOpenApiCorrectly()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost.UseTestServer();
                    webHost.Configure(app => app.Run(async ctx =>
                        await ctx.Response.WriteAsync("Hello World!")));
                });

            // Build and start the IHost
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient to send requests to the TestServer
            var WebApiClient = host.GetTestClient();

            var response = await WebApiClient.GetAsync("/");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello World!", responseString);

            return;

            var builder = WebApplication.CreateBuilder();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
        }
#endif
	}
}