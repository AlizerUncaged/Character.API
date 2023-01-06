using Character.API.Browser;
using Character.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Character.API.CharacterAI;

/// <summary>
/// Class for interacting with the Character.AI API
/// </summary>
public class CharacterAi
{
    private readonly string? _uuid = null;
    public HeadlessBrowserHandler BrowserHandler { get; }
    private string _token;

    /// <summary>
    /// Create a new instance of CharacterAI, this launches a puppeteer-sharp browser.
    /// </summary>
    /// <param name="initializeSynchronously">If true, the connection to the browser will be initialized synchronously. Otherwise, it will be initialized asynchronously.</param>
    /// <param name="uuid">The user's UUID, or the user's unique ID, basically the user's identification for his account.</param>
    public CharacterAi(bool initializeSynchronously = false, string? uuid = null)
    {
        BrowserHandler = new HeadlessBrowserHandler();
        _uuid = uuid;

        if (initializeSynchronously)
        {
            BrowserHandler.InitializeConnectionAsync().GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Initialize the connection to the browser as well as get the authorization token asynchronously.
    /// </summary>
    public async Task InitializeAsync()
    {
        await BrowserHandler.InitializeConnectionAsync();
        await BrowserHandler.SendAsync("https://beta.character.ai/", "", method: HttpMethod.Get);
        var tokenAndUuid = await GetTokenAndUuidAsync();

        _token = tokenAndUuid.Token;
    }

    /// <summary>
    /// Get information about a character by ID.
    /// </summary>
    /// <param name="characterId">The ID of the character to retrieve information for.</param>
    /// <returns>A CharacterInfo object containing information about the character, or null if the request fails.</returns>
    public async Task<CharacterInfo?> GetCharacterInfo(string characterId)
    {
        var response = await BrowserHandler.SendAsync(
            $"{KnownEndpoints.CharacterInfo}/{characterId}/",
            string.Empty, authorization: $"Token {_token}", method: HttpMethod.Get);

        var parsedJson = JsonConvert.DeserializeObject<CharacterInfo>(response);

        return parsedJson;
    }

    /// <summary>
    /// Get the API token and UUID.
    /// </summary>
    /// <returns>A tuple containing the API token and UUID.</returns>
    public async Task<(string Token, string Uuid)> GetTokenAndUuidAsync()
    {
        var uuid = string.IsNullOrWhiteSpace(_uuid) ? Guid.NewGuid().ToString() : _uuid;
        var response = await BrowserHandler.SendAsync(KnownEndpoints.Auth,
            "{\"lazy_uuid\":\"" + uuid + "\"}");

        dynamic jsonResponse = JObject.Parse(response);

        return (jsonResponse.token, jsonResponse.uuid);
    }
}