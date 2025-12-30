using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


using Models.DTO;

namespace Services;

public class LoginServiceWapi : ILoginService
{
    private readonly ILogger<LoginServiceWapi> _logger; 
    private readonly HttpClient _httpClient;
        
    public LoginServiceWapi(ILogger<LoginServiceWapi> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(name: "MusicWebApi");
    }

    public async Task<ResponseItemDto<LoginUserSessionDto>> LoginUserAsync(LoginCredentialsDto usrCreds)
    {
        string uri = $"guest/loginuser";

        //Prepare the request content
        string body = JsonConvert.SerializeObject(usrCreds);
        var requestContent = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PostAsync(uri, requestContent);

        await response.EnsureSuccessStatusMessage();
      
        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<LoginUserSessionDto>>(s);
        return resp;
    }
}

