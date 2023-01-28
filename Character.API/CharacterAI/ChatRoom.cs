using Character.API.Browser;
using PuppeteerSharp;

namespace Character.API.CharacterAI;

public class ChatRoom
{
    // private const string UserInputBox = "#user-input";
    // private const string SendButton = "form.chatform button.py-0:first-of-type";
    // private readonly HeadlessBrowser _headlessBrowser;
    // public string CharacterId { get; }
    //
    // private string _chatUrl;
    //
    // public int CurrentMessageCount { get; set; }
    //
    // /// <summary>
    // /// Create a new ChatRoomHandler for the specified character.
    // /// </summary>
    // /// <param name="characterId">The ID of the character to create the handler for.</param>
    // public ChatRoom(HeadlessBrowser headlessBrowser, string characterId)
    // {
    //     _headlessBrowser = headlessBrowser;
    //     CharacterId = characterId;
    //
    //     _chatUrl = $"{KnownEndpoints.CharacterChatRoom}{characterId}";
    //
    //     _currentPage ??= headlessBrowser.Browser?.NewPageAsync().GetAwaiter().GetResult();
    //
    //     _currentPage.GoToAsync(_chatUrl).GetAwaiter().GetResult();
    // }
    //
    // async Task AwaitInputFormsAsync()
    // {
    //     while (true)
    //     {
    //         var userInputBox = await _currentPage.EvaluateExpressionAsync<bool>(@$"
    //             document.querySelector('{UserInputBox}') !== null;
    //         ");
    //
    //         if (userInputBox)
    //             return;
    //         else
    //             await Task.Delay(TimeSpan.FromMilliseconds(100));
    //     }
    // }
    //
    // public Task NewMessageWatcher()
    // {
    //     int currentReadMessages = CurrentMessageCount;
    //     while (true)
    //     {
    //         // Keep reading the amount of new message lengths.
    //     }
    // }
    //
    // /// <summary>
    // /// Send a message to the chat room.
    // /// </summary>
    // /// <param name="message">The message to send.</param>
    // /// <returns>The response from the character.</returns>
    // public async Task<string> SendMessageAsync(string message)
    // {
    //     await AwaitInputFormsAsync();
    //     await _currentPage.FocusAsync("#user-input");
    //     await _currentPage.Keyboard.TypeAsync(message);
    //
    //     // Get the submit button.
    //     // await _currentPage.ClickAsync("form.chatform > button:first-of-type");
    //     await _currentPage.EvaluateExpressionAsync(@$"
    //             document.querySelector('{SendButton}').click()
    //         ");
    //
    //     await Task.Delay(TimeSpan.FromSeconds(1));
    //
    //     return null;
    // }
}