using GitHubActionsSpike.WebApplication1;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace GitHubActionsSpike.Tests;

public class UnitTest1(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
	private static readonly WebApplicationFactoryClientOptions _options = new() { AllowAutoRedirect = false, };
	private readonly HttpClient _httpClient = factory.CreateClient(_options);

	[Theory]
	[InlineData("weatherforecast")]
	public async Task Test1(string requestUri)
	{
		var response = await _httpClient.GetAsync(requestUri);
		var content = await response.Content.ReadAsStringAsync();

		Assert.True(
			response.IsSuccessStatusCode,
			userMessage: response.StatusCode + " " + content);

		Assert.NotEmpty(content);
		Assert.StartsWith("[", content);

		var forecasts = JsonSerializer.Deserialize<WeatherForecast[]>(content);

		Assert.NotNull(forecasts);
		Assert.NotEmpty(forecasts);
		Assert.DoesNotContain(null, forecasts);
	}
}
