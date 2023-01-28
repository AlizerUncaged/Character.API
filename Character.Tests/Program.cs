using Character.API;
using Character.API.CharacterAI;
using Character.Tests;

// Character ID of the character to chat on.
const string chatTarget = "Mg4vmEpJ4gNxw416vdIlvaNedXEV6nAGzXbdDIA-xzU";

// Messages to send.
string[] chatMessages = new[] { "Hello there! What's good?"};

var characterAi = new CharacterAi();

Console.WriteLine("Initializing connections...");
characterAi.Browser.Fetcher.DownloadProgressChanged += (sender, eventArgs) =>
{
    Console.Write(
        $"\rDownloading Selenium...{eventArgs.BytesReceived.FormatBytes()} out of {eventArgs.TotalBytesToReceive.FormatBytes()}  ");
};

await characterAi.InitializeAsync();

Console.WriteLine(" OK! 🎉 We can now chat!");

Console.WriteLine($"Getting character info of {chatTarget}...");

var characterInfo = await characterAi.GetCharacterInfo(chatTarget);

if (characterInfo is null)
{
    throw new NullReferenceException(
        $"Character info was null, we were unable to get the character info with the ID of {chatTarget}");
}

Console.WriteLine($"Fetched! ✔️ {characterInfo.Character}");

Console.WriteLine($"\nNow opening a chat room for {characterInfo.Name}! 💬");
//
// var chatRoom = await characterAi.CreateChatRoom(characterInfo);
//
// Console.WriteLine($"\nChat room opened! 💎");
//
// foreach (var message in chatMessages)
// {
//     await chatRoom.SendMessageAsync(message);
// }