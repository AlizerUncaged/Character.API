using Character.API.Browser;
using Character.API.CharacterAI;
using Character.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Character.API;

/// <summary>
/// Class for interacting with the Character.AI API
/// </summary>
public class CharacterAi
{
    private readonly string? _uuid = null;

    public HeadlessBrowser Browser { get; }

    private string? _token;

    /// <summary>
    /// Create a new instance of CharacterAI, this launches a puppeteer-sharp browser.
    /// </summary>
    /// <param name="initializeSynchronously">If true, the connection to the browser will be initialized synchronously.
    /// Otherwise, it should be initialized asynchronously via InitializeAsync().</param>
    /// <param name="uuid">The user's UUID, or the user's unique ID, basically the user's identification for his account.
    /// </param>
    public CharacterAi(bool initializeSynchronously = false, string? uuid = null)
    {
        Browser ??= new HeadlessBrowser();
        _uuid = uuid;

        if (initializeSynchronously)
        {
            Browser.InitializeConnectionAsync().GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Initialize the connection to the browser as well as get the authorization token.
    /// </summary>
    public async Task InitializeAsync()
    {
        // Initialize the connection to the browser.
        await Browser.InitializeConnectionAsync();

        // Send a request to the Character.AI.
        await Browser.SendAsync("https://beta.character.ai/", "", method: HttpMethod.Get);
    }

    public async Task<ChatRoom?> CreateChatRoom(CharacterInfo characterInfo) =>
        await CreateChatRoom(characterInfo.Character.ExternalId);
    
    public async Task<ChatRoom?> CreateChatRoom(string characterId)
    {
        ChatRoom? chatRoom = null;
        //
        // await Task.Run(() => { chatRoom = new ChatRoom(Browser, characterId); });
        //
        return chatRoom;
    }

    /// <summary>
    /// Get information about a character by ID.
    /// </summary>
    /// <param name="characterId">The ID of the character to retrieve information for.</param>
    /// <returns>A CharacterInfo object containing information about the character, or null if the request fails.</returns>
    public async Task<CharacterInfo?> GetCharacterInfo(string characterId)
    {
        // Make an HTTP GET request to the Character Info endpoint and pass in the character ID as a URL parameter.
        var response = await Browser.SendAsync(
            $"{KnownEndpoints.CharacterInfo}/{characterId}/",
            string.Empty, authorization: $"Token {_token}", method: HttpMethod.Get);

        // Deserialize the response from the server into a CharacterInfo object.
        var parsedJson = JsonConvert.DeserializeObject<CharacterInfo>(response);

        // Return the CharacterInfo object.
        return parsedJson;
    }

    // /// <summary>
    // /// Get the API token.
    // /// </summary>
    // /// <returns>A tuple containing the API token and UUID.</returns>
    // public async Task<(string Token, string Uuid)> GetTokenAsync()
    // {
    //     // If the UUID is not provided, generate a new one using Guid.NewGuid().
    //     // Otherwise, use the provided UUID.
    //     var uuid = string.IsNullOrWhiteSpace(_uuid) ? Guid.NewGuid().ToString() : _uuid;
    //
    //     // Send a request to the "Auth" endpoint to get the token.
    //     var response = await Browser.SendAsync(KnownEndpoints.Auth,
    //         "{\"lazy_uuid\":\"" + uuid + "\"}", method: HttpMethod.Post);
    //
    //     // Parse the JSON response.
    //     dynamic jsonResponse = JObject.Parse(response);
    //
    //     // Return the token and UUID.
    //     return (jsonResponse.token, jsonResponse.uuid);
    // }
}