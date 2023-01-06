using Character.API.Browser;
using PuppeteerSharp;

namespace Character.API.CharacterAI;

public class ChatRoomHandler
{
    private readonly HeadlessBrowser _headlessBrowser;
    public string CharacterId { get; }

    private IPage? _currentPage;

    private string _chatUrl;

    /// <summary>
    /// Create a new ChatRoomHandler for the specified character.
    /// </summary>
    /// <param name="characterId">The ID of the character to create the handler for.</param>
    public ChatRoomHandler(HeadlessBrowser headlessBrowser, string characterId)
    {
        _headlessBrowser = headlessBrowser;
        CharacterId = characterId;

        _chatUrl = $"{KnownEndpoints.CharacterChatRoom}{characterId}";

        _currentPage ??= headlessBrowser.Browser?.NewPageAsync().GetAwaiter().GetResult();

        _currentPage.GoToAsync(_chatUrl).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Send a message to the chat room.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>The response from the character.</returns>
    public async Task<string> SendMessageAsync(string message)
    {
        throw new Exception();
    }
}