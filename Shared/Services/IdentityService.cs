namespace Shared.Services;

public class IdentityService : IIdentityService
{
    private const string Url = "https://localhost:7000/companies/validate";
    private readonly HttpClient _httpClient;

    public IdentityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int?> ValidateCompanyCredentialsAsync(string apiKey, string apiSecret)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, Url);
        request.Headers.Add("ApiKey", apiKey);
        request.Headers.Add("ApiSecret", apiSecret);

        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var companyId = await response.Content.ReadAsStringAsync();
            return int.Parse(companyId);
        }
        return null;
    }
}
